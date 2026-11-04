using Apex.Portfolio.Abstracts.List;
using Apex.Portfolio.Features.Modules.BondOperations.List.Filtering;
using Riok.Mapperly.Abstractions;
using BondOperationEntity = Apex.Portfolio.Domain.Entities.Bond.BondOperation;

namespace Apex.Portfolio.Features.Modules.BondOperations.List.Models;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class BondOperationListModelMapper :
    IBaseListMapper<BondOperationListModel, BondOperationEntity, BondOperationListFilter, BondOperationListQuery>
{
#pragma warning disable RMG020
#pragma warning disable RMG012

    public partial List<BondOperationListModel> MapToModel(List<BondOperationEntity> entity);
    public partial BondOperationListQuery MapToQuery(BondOperationListFilter filter);

#pragma warning restore RMG020
#pragma warning restore RMG012
}
