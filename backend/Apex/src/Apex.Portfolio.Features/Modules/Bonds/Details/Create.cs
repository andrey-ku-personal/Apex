using System.Security.Claims;
using Apex.Portfolio.Factories;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using Riok.Mapperly.Abstractions;
using BondEntity = Apex.Portfolio.Domain.Entities.Bond.Bond;
using ValidationException = Apex.Shared.Core.Exceptions.ValidationException;

namespace Apex.Portfolio.Features.Modules.Bonds.Details;

public class CreateRequest : BondModel;

public class CreateRequestValidator : AbstractValidator<CreateRequest>
{
    public CreateRequestValidator()
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

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class BondCreateMapper
{
#pragma warning disable RMG020
#pragma warning disable RMG012

    [MapperIgnoreTarget(nameof(BondEntity.Id))]
    public partial BondEntity MapToEntity(CreateRequest request);

#pragma warning restore RMG020
#pragma warning restore RMG012
}

public class CreateHandler(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory,
    IValidationRunner validation)
    : CreateHandler<BondModel, CreateRequest, BondEntity>(dbFactory, validation)
{
    private readonly BondCreateMapper createMapper = mapperFactory.GetMapper<BondCreateMapper>();
    private readonly BondMapper mapper = mapperFactory.GetMapper<BondMapper>();

    protected override BondEntity MapToEntity(CreateRequest request)
        => createMapper.MapToEntity(request);

    protected override BondModel MapToModel(BondEntity entity)
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
    [HttpPost("Bond/Create")]
    [OpenApiOperation(
        operationId: "Bond.Create",
        summary: "Create Bond",
        description: "Create new Bond for the current user")]
    [OpenApiTags("Bond")]
    public async Task<BondModel> Create([FromBody] CreateRequest request, CancellationToken cancellationToken = default)
    {
        request.UserId = User.FindFirstValue("sub")!;
        return await handler.Handle(request, cancellationToken);
    }
}
