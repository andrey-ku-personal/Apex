using Apex.Shared.Core.Pagination.Models;

namespace Apex.Portfolio.Features.Modules.Bonds.List.Filtering;

public class BondListFilter : PageFilter
{
    public int? CurrencyId { get; set; }
    public int? PaymentFrequencyId { get; set; }
}