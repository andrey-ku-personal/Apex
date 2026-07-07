using Apex.Invest.Domain.Enums;

namespace Apex.Invest.Domain.Entities;

public class Share : FinanceInstrument
{
    public Currency Currency { get; set; }
    public decimal MarketPrice { get; set; }
    public int Quantity { get; set; }
    public decimal BrokerCommission { get; set; }
    public decimal DividendRate { get; set; }
}
