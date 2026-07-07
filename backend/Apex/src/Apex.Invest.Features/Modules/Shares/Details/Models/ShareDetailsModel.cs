using Apex.Invest.Domain.Enums;

namespace Apex.Invest.Features.Modules.Shares.Details.Models;

public class ShareDetailsModel
{
    public int Id { get; set; }
    public int PlatformId { get; set; }
    public string Ticker { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public DateTime PurchaseDate { get; set; }
    public Status Status { get; set; }

    public Currency Currency { get; set; }
    public decimal MarketPrice { get; set; }
    public int Quantity { get; set; }
    public decimal BrokerCommission { get; set; }
    public decimal DividendRate { get; set; }
}
