using Apex.Portfolio.Abstracts.Details;
using Apex.Portfolio.Features.Modules.Bonds.Details.Filtering;
using Riok.Mapperly.Abstractions;
using BondEntity = Apex.Portfolio.Domain.Entities.Bond.Bond;

namespace Apex.Portfolio.Features.Modules.Bonds.Details.Models;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class BondModelMapper : IBaseDetailsMapper<BondModel, BondEntity, BondFilter, BondQuery>
{
#pragma warning disable RMG020
#pragma warning disable RMG012

    public partial BondModel MapToModel(BondEntity entity);
    public partial BondFilter MapToFilter(BondEntity entity);
    public partial BondQuery MapToQuery(BondFilter filter);

    public BondEntity MapToEntity(BondModel model) => new()
    {
        Ticker = model.Ticker,
        Issuer = model.Issuer,
        CurrencyId = model.CurrencyId,
        PaymentFrequencyId = model.PaymentFrequencyId,
        IssueDate = model.IssueDate,
        MaturityDate = model.MaturityDate,
        PaymentDay = model.PaymentDay,
        ParPrice = model.ParPrice,
        Rate = model.Rate,
        IsRefinancingLinked = model.IsRefinancingLinked
    };

    public BondEntity MapData(BondModel model, BondEntity entity)
    {
        entity.Ticker = model.Ticker;
        entity.Issuer = model.Issuer;
        entity.CurrencyId = model.CurrencyId;
        entity.PaymentFrequencyId = model.PaymentFrequencyId;
        entity.IssueDate = model.IssueDate;
        entity.MaturityDate = model.MaturityDate;
        entity.PaymentDay = model.PaymentDay;
        entity.ParPrice = model.ParPrice;
        entity.Rate = model.Rate;
        entity.IsRefinancingLinked = model.IsRefinancingLinked;

        return entity;
    }

#pragma warning restore RMG020
#pragma warning restore RMG012
}