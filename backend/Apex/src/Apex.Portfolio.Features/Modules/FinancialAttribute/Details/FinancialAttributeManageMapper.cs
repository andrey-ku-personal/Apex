using Apex.Portfolio.Abstracts.Details;
using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Filters;
using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Models;
using Riok.Mapperly.Abstractions;

namespace Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Mapper;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class FinancialAttributeManageMapper : IBaseDetailsMapper<FinancialAttributeModel, Domain.Abstractions.FinancialAttribute, FinancialAttributeFilter, FinancialAttributeQuery>
{
#pragma warning disable RMG020
#pragma warning disable RMG012

    public partial FinancialAttributeModel MapToModel(Domain.Abstractions.FinancialAttribute entity);
    public partial FinancialAttributeFilter MapToFilter(Domain.Abstractions.FinancialAttribute entity);
    public partial FinancialAttributeQuery MapToQuery(FinancialAttributeFilter filter);

#pragma warning restore RMG020
#pragma warning restore RMG012

    public Domain.Abstractions.FinancialAttribute MapData(FinancialAttributeModel model, Domain.Abstractions.FinancialAttribute entity)
    {
        entity.Name = model.Name;

        return entity;
    }
}
