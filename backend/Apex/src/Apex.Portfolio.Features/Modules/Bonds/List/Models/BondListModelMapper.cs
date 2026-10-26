using Apex.Portfolio.Abstracts.List;
using Apex.Portfolio.Features.Modules.Bonds.List.Filtering;
using Riok.Mapperly.Abstractions;
using BondEntity = Apex.Portfolio.Domain.Entities.Bond.Bond;

namespace Apex.Portfolio.Features.Modules.Bonds.List.Models;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class BondListModelMapper :
    IBaseListMapper<BondListModel, BondEntity, BondListFilter, BondListQuery>
{
#pragma warning disable RMG020
#pragma warning disable RMG012

    public partial List<BondListModel> MapToModel(List<BondEntity> entity);
    public partial BondListQuery MapToQuery(BondListFilter filter);

#pragma warning restore RMG020
#pragma warning restore RMG012
}