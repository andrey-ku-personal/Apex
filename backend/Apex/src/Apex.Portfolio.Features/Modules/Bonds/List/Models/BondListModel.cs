namespace Apex.Portfolio.Features.Modules.Bonds.List.Models;

public class BondListModel
{
    public int Id { get; set; }
    public string Ticker { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public int CurrencyId { get; set; }
    public int PaymentFrequencyId { get; set; }
    public decimal Rate { get; set; }
    public DateOnly MaturityDate { get; set; }
}