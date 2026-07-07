using Apex.Invest.Abstracts.Details;
using Apex.Invest.Domain.Entities;
using Apex.Invest.Features.Modules.Bonds.Details.Filters;
using Apex.Invest.Features.Modules.Bonds.Details.Models;
using Apex.Invest.Features.Modules.Bonds.Details.Queries;
using Riok.Mapperly.Abstractions;

namespace Apex.Invest.Features.Modules.Bonds.Details.Mapper;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class BondDetailsMapper : IBaseDetailsMapper<BondDetailsModel, Bond, BondDetailsFilter, BondDetailsQuery>
{
#pragma warning disable RMG020
#pragma warning disable RMG012
    public partial BondDetailsModel MapToModel(Bond entity);
    public partial BondDetailsFilter MapToFilter(Bond entity);
    public partial BondDetailsQuery MapToQuery(BondDetailsFilter filter);

    public partial void UpdateEntity(BondDetailsModel model, Bond entity);

    public Bond MapData(BondDetailsModel model, Bond entity)
    {
        UpdateEntity(model, entity);

        return entity;
    }

#pragma warning restore RMG020
#pragma warning restore RMG012
}
