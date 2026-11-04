using Apex.Portfolio.Domain.Enums;
using Apex.Shared.Core.Pagination.Models;

namespace Apex.Portfolio.Features.Modules.BondOperations.List.Filtering;

public class BondOperationListFilter : PageFilter
{
    public string UserId { get; set; } = null!;
    public int? BondId { get; set; }
    public OperationType? Type { get; set; }
    public DateOnly? DateFrom { get; set; }
    public DateOnly? DateTo { get; set; }
}
