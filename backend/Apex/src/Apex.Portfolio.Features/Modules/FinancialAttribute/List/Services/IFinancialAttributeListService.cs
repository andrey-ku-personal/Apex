using Apex.Portfolio.Abstracts.List;
using Apex.Portfolio.Features.Modules.FinancialAttribute.List.Filtering;
using Apex.Portfolio.Features.Modules.FinancialAttribute.List.Models;
namespace Apex.Portfolio.Features.Modules.FinancialAttribute.List.Services;

public interface IFinancialAttributeListService : IBaseListService<FinancialAttributeListModel, Domain.Abstractions.FinancialAttribute, FinancialAttributeListFilter, FinancialAttributeListQuery>;
