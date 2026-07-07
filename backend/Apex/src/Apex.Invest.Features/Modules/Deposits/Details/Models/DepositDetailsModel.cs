using Apex.Invest.Domain.Enums;

namespace Apex.Invest.Features.Modules.Deposits.Details.Models;

public class DepositDetailsModel
{
    public int Id { get; set; }
    public int PlatformId { get; set; }
    public string Ticker { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public DateTime PurchaseDate { get; set; }
    public Status Status { get; set; }

    public decimal InterestRate { get; set; }
    public PaymentFrequence Frequence { get; set; }
    public bool IsReplenishable { get; set; }
    public DateTime FirstPaymentDate { get; set; }
    public DateTime MaturityDate { get; set; }
    public DateTime? SaleDate { get; set; }
    public bool IsTaxFree { get; set; }
    public bool Capitalization { get; set; }
}
