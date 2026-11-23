using System.Security.Claims;
using Apex.Portfolio.Factories;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Core.Exceptions;
using Apex.Shared.Core.Pagination;
using Apex.Shared.Core.Pagination.Expressions;
using Apex.Shared.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using BondEntity = Apex.Portfolio.Domain.Entities.Bond.Bond;
using ValidationException = Apex.Shared.Core.Exceptions.ValidationException;

namespace Apex.Portfolio.Features.Modules.Bonds.Details;

public class UpdateRequest : BondModel;

public class UpdateRequestValidator : AbstractValidator<UpdateRequest>
{
    public UpdateRequestValidator()
    {
        RuleFor(x => x.Ticker).NotNull().NotEmpty().MaximumLength(64);
        RuleFor(x => x.Issuer).NotNull().NotEmpty().MaximumLength(256);
        RuleFor(x => x.CurrencyId).GreaterThan(0);
        RuleFor(x => x.PaymentFrequencyId).GreaterThan(0);
        RuleFor(x => x.PaymentDay).InclusiveBetween(1, 31);
        RuleFor(x => x.Rate).GreaterThanOrEqualTo(0).PrecisionScale(9, 4, ignoreTrailingZeros: false);
        RuleFor(x => x.ParPrice).GreaterThan(0).PrecisionScale(18, 2, ignoreTrailingZeros: false);
        RuleFor(x => x.MaturityDate).GreaterThan(x => x.IssueDate);
    }
}

public class BondUpdateMapper
{
    public BondEntity MapData(UpdateRequest request, BondEntity entity)
    {
        entity.Ticker = request.Ticker;
        entity.Issuer = request.Issuer;
        entity.CurrencyId = request.CurrencyId;
        entity.PaymentFrequencyId = request.PaymentFrequencyId;
        entity.IssueDate = request.IssueDate;
        entity.MaturityDate = request.MaturityDate;
        entity.PaymentDay = request.PaymentDay;
        entity.ParPrice = request.ParPrice;
        entity.Rate = request.Rate;
        entity.IsRefinancingLinked = request.IsRefinancingLinked;

        return entity;
    }
}

public class UpdateHandler(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory,
    IValidationRunner validation)
{
    private readonly BondUpdateMapper updateMapper = mapperFactory.GetMapper<BondUpdateMapper>();
    private readonly BondMapper mapper = mapperFactory.GetMapper<BondMapper>();

    public async Task<BondModel> Handle(int id, string userId, UpdateRequest request, CancellationToken cancellationToken)
    {
        await Validate(id, userId, request, cancellationToken);

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        await db.BeginTransactionAsync(cancellationToken);

        var entity = await db.Set<BondEntity>()
            .ByQuery(new BondQuery { Id = id, UserId = userId })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"Bond was not found {id}");

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
    [HttpPut("Bond/Update/{id:int}")]
    [OpenApiOperation(
        operationId: "Bond.Update",
        summary: "Update Bond",
        description: "Update own Bond by id")]
    [OpenApiTags("Bond")]
    public async Task<BondModel> Update([FromRoute] int id, [FromBody] UpdateRequest request, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue("sub")!;
        return await handler.Handle(id, userId, request, cancellationToken);
    }
}
