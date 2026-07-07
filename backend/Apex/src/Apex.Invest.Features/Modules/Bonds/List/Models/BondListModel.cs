using Apex.Invest.Domain.Enums;

namespace Apex.Invest.Features.Modules.Bonds.List.Models;

public class BondListModel
{
    public int Id { get; set; }
    public string Ticker { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public decimal InterestRate { get; set; }
    public Currency Currency { get; set; }
    public decimal MarketPrice { get; set; }
    public int Quantity { get; set; }
    public Status Status { get; set; }
}
