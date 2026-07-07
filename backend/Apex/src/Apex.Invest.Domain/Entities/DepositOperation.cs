using Apex.Invest.Domain.Abstractions;
using Apex.Invest.Domain.Enums;

namespace Apex.Invest.Domain.Entities;

public class DepositOperation : AnalyticalEntity
{
    public int Id { get; set; }
    public int DepositId { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public DepositOperationType Type { get; set; }

    public virtual Deposit Deposit { get; set; } = default!;
}
