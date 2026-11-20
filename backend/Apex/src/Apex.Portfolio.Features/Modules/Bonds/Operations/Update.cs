using System.Security.Claims;
using Apex.Portfolio.Factories;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Core.Exceptions;
using Apex.Shared.Core.Pagination.Expressions;
using Apex.Shared.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using BondOperationEntity = Apex.Portfolio.Domain.Entities.Bond.BondOperation;
using ValidationException = Apex.Shared.Core.Exceptions.ValidationException;
using DateTime = System.DateTime;

namespace Apex.Portfolio.Features.Modules.Bonds.Operations;

public class UpdateRequest : BondOperationModel
{
}

public class UpdateRequestValidator : AbstractValidator<UpdateRequest>
{
    public UpdateRequestValidator()
    {
        RuleFor(x => x.BondId).GreaterThan(0);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Date).NotEmpty()
            .LessThanOrEqualTo(_ => DateOnly.FromDateTime(DateTime.UtcNow));
        RuleFor(x => x.Count).GreaterThan(0);
        RuleFor(x => x.Price).GreaterThan(0).PrecisionScale(18, 2, ignoreTrailingZeros: false);
        RuleFor(x => x.Commission).GreaterThanOrEqualTo(0).PrecisionScale(18, 2, ignoreTrailingZeros: false);
    }
}

public class BondOperationUpdateMapper
{
    public BondOperationEntity MapData(UpdateRequest request, BondOperationEntity entity)
    {
        entity.BondId = request.BondId;
        entity.UserId = request.UserId;
        entity.Type = request.Type;
        entity.Date = request.Date;
        entity.Count = request.Count;
        entity.Price = request.Price;
        entity.Commission = request.Commission;

        return entity;
    }
}

public class UpdateHandler(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory,
    IValidationRunner validation)
{
    private readonly BondOperationUpdateMapper updateMapper = mapperFactory.GetMapper<BondOperationUpdateMapper>();
    private readonly BondOperationMapper mapper = mapperFactory.GetMapper<BondOperationMapper>();

    public async Task<BondOperationModel> Handle(int id, string userId, UpdateRequest request, CancellationToken cancellationToken)
    {
        await Validate(id, userId, request, cancellationToken);

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        await db.BeginTransactionAsync(cancellationToken);

        var entity = await db.Set<BondOperationEntity>()
            .ByQuery(new BondOperationQuery { Id = id, UserId = userId })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"BondOperation was not found {id}");

        updateMapper.MapData(request, entity);

        await db.CommitTransactionAsync(cancellationToken);

        return mapper.MapToModel(entity);
    }

    private async Task Validate(int id, string userId, UpdateRequest request, CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, string[]>();

        if (id < 1)
            errors["Id"] = ["'Id' must be greater than '0'."];
        if (string.IsNullOrWhiteSpace(userId))
            errors["UserId"] = ["'UserId' must not be empty."];
        if (errors.Count > 0)
            throw new ValidationException(errors);

        await validation.ValidateAsync(request, cancellationToken);
    }
}

[Authorize]
public class UpdateController(UpdateHandler handler) : SharedController
{
    [HttpPut("BondOperation/Update/{id:int}")]
    [OpenApiOperation(
        operationId: "BondOperation.Update",
        summary: "Update BondOperation",
        description: "Update own operation by id (position cannot go negative)")]
    [OpenApiTags("BondOperation")]
    public async Task<BondOperationModel> Update([FromRoute] int id, [FromBody] UpdateRequest request, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue("sub")!;
        request.UserId = userId;
        return await handler.Handle(id, userId, request, cancellationToken);
    }
}
