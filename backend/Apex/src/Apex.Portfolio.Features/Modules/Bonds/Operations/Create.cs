using System.Security.Claims;
using Apex.Portfolio.Factories;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Core.Exceptions;
using Apex.Shared.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using Riok.Mapperly.Abstractions;
using BondOperationEntity = Apex.Portfolio.Domain.Entities.Bond.BondOperation;
using ValidationException = Apex.Shared.Core.Exceptions.ValidationException;
using DateTime = System.DateTime;

namespace Apex.Portfolio.Features.Modules.Bonds.Operations;

public class CreateRequest : BondOperationModel
{
}

public class CreateRequestValidator : AbstractValidator<CreateRequest>
{
    public CreateRequestValidator()
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

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class BondOperationCreateMapper
{
#pragma warning disable RMG020
#pragma warning disable RMG012

    [MapperIgnoreTarget(nameof(BondOperationEntity.Id))]
    public partial BondOperationEntity MapToEntity(CreateRequest request);

#pragma warning restore RMG020
#pragma warning restore RMG012
}

public class CreateHandler(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory,
    IValidationRunner validation)
    : CreateHandler<BondOperationModel, CreateRequest, BondOperationEntity>(dbFactory, validation)
{
    private readonly BondOperationCreateMapper createMapper = mapperFactory.GetMapper<BondOperationCreateMapper>();
    private readonly BondOperationMapper mapper = mapperFactory.GetMapper<BondOperationMapper>();

    protected override BondOperationEntity MapToEntity(CreateRequest request)
        => createMapper.MapToEntity(request);

    protected override BondOperationModel MapToModel(BondOperationEntity entity)
        => mapper.MapToModel(entity);

    protected override async Task Validate(CreateRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
            throw new ValidationException(new Dictionary<string, string[]>
            { ["UserId"] = ["'UserId' must not be empty."] });

        await Validation.ValidateAsync(request, cancellationToken);
    }
}

[Authorize]
public class CreateController(CreateHandler handler) : SharedController
{
    [HttpPost("BondOperation/Create")]
    [OpenApiOperation(
        operationId: "BondOperation.Create",
        summary: "Create BondOperation",
        description: "Create buy or sell operation for the current user")]
    [OpenApiTags("BondOperation")]
    public async Task<BondOperationModel> Create([FromBody] CreateRequest request, CancellationToken cancellationToken = default)
    {
        request.UserId = User.FindFirstValue("sub")!;
        return await handler.Handle(request, cancellationToken);
    }
}
