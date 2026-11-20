using Apex.Portfolio.Factories;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Core.Pagination;
using Apex.Shared.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using BondEntity = Apex.Portfolio.Domain.Entities.Bond.Bond;

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
    : UpdateHandler<BondModel, UpdateRequest, BondEntity>(dbFactory, validation)
{
    private readonly BondUpdateMapper updateMapper = mapperFactory.GetMapper<BondUpdateMapper>();
    private readonly BondMapper mapper = mapperFactory.GetMapper<BondMapper>();

    protected override BaseQuery<BondEntity> GetQuery(int id)
        => new BondQuery { Id = id };

    protected override void MapData(UpdateRequest request, BondEntity entity)
        => updateMapper.MapData(request, entity);

    protected override BondModel MapToModel(BondEntity entity)
        => mapper.MapToModel(entity);
}

public class UpdateController(UpdateHandler handler) : SharedController
{
    [HttpPut("Bond/Update/{id:int}")]
    [OpenApiOperation(
        operationId: "Bond.Update",
        summary: "Update Bond",
        description: "Update Bond by id")]
    [OpenApiTags("Bond")]
    public async Task<BondModel> Update([FromRoute] int id, [FromBody] UpdateRequest request, CancellationToken cancellationToken = default)
        => await handler.Handle(id, request, cancellationToken);
}
