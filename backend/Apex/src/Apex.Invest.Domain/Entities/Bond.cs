using Apex.Invest.Domain.Enums;

namespace Apex.Invest.Domain.Entities;

public class Bond : FinanceInstrument
{
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
