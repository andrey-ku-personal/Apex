namespace Apex.Portfolio.Features.Modules.Bonds.Details.Models;

public class BondModel
{
    public int Id { get; set; }
    public string Ticker { get; set; } = default!;
    public string Issuer { get; set; } = default!;

    public int CurrencyId { get; set; }
    public int PaymentFrequencyId { get; set; }

    public DateOnly IssueDate { get; set; }
    public DateOnly MaturityDate { get; set; }
    public int PaymentDay { get; set; }

    public decimal ParPrice { get; set; }
    public decimal Rate { get; set; }
    public bool IsRefinancingLinked { get; set; }
}