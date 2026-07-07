using Apex.Invest.Domain.Enums;

namespace Apex.Invest.Domain.Entities;

public class Deposit : FinanceInstrument
{
    public decimal InterestRate { get; set; }
    public PaymentFrequence Frequence { get; set; }
    public bool IsReplenishable { get; set; }
    public DateTime FirstPaymentDate { get; set; }
    public DateTime MaturityDate { get; set; }
    public DateTime? SaleDate { get; set; }
    public bool IsTaxFree { get; set; }
    public bool Capitalization { get; set; }

    public virtual ICollection<DepositOperation> Operations { get; set; } = [];
}
