using Apex.Invest.Domain.Enums;

namespace Apex.Invest.Domain.Entities.Bond;

public class Bond : FinanceInstrument
{
    public Currency Currency { get; set; }
    public decimal ParPrice { get; set; }
    public decimal CouponRate { get; set; }
    public PaymentFrequence PaymentFrequence { get; set; }
    public int NextCouponDate { get; set; }
    public DateTime MaturityDate { get; set; }
    
    public virtual ICollection<BondOperation>? Operations { get; set; }
}
