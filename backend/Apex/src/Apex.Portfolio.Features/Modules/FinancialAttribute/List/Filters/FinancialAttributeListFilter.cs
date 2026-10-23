using Apex.Portfolio.Domain.Enums;
using Apex.Shared.Core.Pagination.Models;

namespace Apex.Portfolio.Features.Modules.FinancialAttribute.List.Filters;

public class FinancialAttributeListFilter : PageFilter
{
    public MetadataType? Type { get; set; }
}
