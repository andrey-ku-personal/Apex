using Apex.Portfolio.Abstracts.List;
using Apex.Portfolio.Features.Modules.FinancialAttribute.List.Filters;
using Apex.Portfolio.Features.Modules.FinancialAttribute.List.Models;
using Riok.Mapperly.Abstractions;

namespace Apex.Portfolio.Features.Modules.FinancialAttribute.List.Mapper;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class FinancialAttributeListMapper :
    IBaseListMapper<FinancialAttributeListModel, Domain.Abstractions.FinancialAttribute, FinancialAttributeListFilter, FinancialAttributeListQuery>
{
#pragma warning disable RMG020
#pragma warning disable RMG012

    public partial List<FinancialAttributeListModel> MapToModel(List<Domain.Abstractions.FinancialAttribute> entity);
    public partial FinancialAttributeListQuery MapToQuery(FinancialAttributeListFilter filter);

#pragma warning restore RMG020
#pragma warning restore RMG012
}
