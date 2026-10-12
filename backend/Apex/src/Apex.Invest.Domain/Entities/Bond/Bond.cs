using Apex.Invest.Domain.Enums;

namespace Apex.Invest.Domain.Entities.Bond;

public class Bond : FinanceInstrument
{
    public Currency Currency { get; set; }
    public decimal ParPrice { get; set; }
    public decimal CouponRate { get; set; }
    public PaymentFrequency PaymentFrequency { get; set; }
    public int NextCouponDate { get; set; }
    public DateOnly MaturityDate { get; set; }
    
    public virtual ICollection<BondOperation>? Operations { get; set; }
}
