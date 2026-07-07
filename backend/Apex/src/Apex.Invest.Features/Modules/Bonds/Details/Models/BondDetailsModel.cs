using Apex.Invest.Domain.Enums;

namespace Apex.Invest.Features.Modules.Bonds.Details.Models;

public class BondDetailsModel
{
    public int Id { get; set; }
    public int PlatformId { get; set; }
    public string Ticker { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public DateTime PurchaseDate { get; set; }
    public Status Status { get; set; }

    public decimal InterestRate { get; set; }
    public Currency Currency { get; set; }
    public PaymentFrequence Frequence { get; set; }
    public decimal NominalPrice { get; set; }
    public decimal MarketPrice { get; set; }
    public int Quantity { get; set; }
    public decimal BrokerCommission { get; set; }
    public DateTime FirstPaymentDate { get; set; }
    public DateTime MaturityDate { get; set; }
    public DateTime? SaleDate { get; set; }
}
