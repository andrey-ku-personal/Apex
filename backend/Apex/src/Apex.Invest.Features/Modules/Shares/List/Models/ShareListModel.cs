using Apex.Invest.Domain.Enums;

namespace Apex.Invest.Features.Modules.Shares.List.Models;

public class ShareListModel
{
    public int Id { get; set; }
    public string Ticker { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public Currency Currency { get; set; }
    public decimal MarketPrice { get; set; }
    public int Quantity { get; set; }
    public decimal DividendRate { get; set; }
    public Status Status { get; set; }
}
