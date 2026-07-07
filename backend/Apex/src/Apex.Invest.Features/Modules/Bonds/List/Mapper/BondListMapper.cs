using Apex.Invest.Abstracts.List;
using Apex.Invest.Domain.Entities;
using Apex.Invest.Features.Modules.Bonds.List.Filters;
using Apex.Invest.Features.Modules.Bonds.List.Models;
using Apex.Invest.Features.Modules.Bonds.List.Queries;
using Riok.Mapperly.Abstractions;

namespace Apex.Invest.Features.Modules.Bonds.List.Mapper;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class BondListMapper : IBaseListMapper<BondListModel, Bond, BondListFilter, BondListQuery>
{
#pragma warning disable RMG020
#pragma warning disable RMG012

    public partial List<BondListModel> MapToModel(List<Bond> entity);
    public partial BondListQuery MapToQuery(BondListFilter filter);

#pragma warning restore RMG020
#pragma warning restore RMG012
}
