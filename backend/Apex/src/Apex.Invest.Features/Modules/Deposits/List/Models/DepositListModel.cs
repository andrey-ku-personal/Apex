using Apex.Invest.Domain.Enums;

namespace Apex.Invest.Features.Modules.Deposits.List.Models;

public class DepositListModel
{
    public int Id { get; set; }
    public string Ticker { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public decimal InterestRate { get; set; }
    public PaymentFrequence Frequence { get; set; }
    public bool IsReplenishable { get; set; }
    public bool IsTaxFree { get; set; }
    public bool Capitalization { get; set; }
    public Status Status { get; set; }
}
