using Apex.Invest.Domain.Abstractions;
using Apex.Invest.Domain.Enums;

namespace Apex.Invest.Domain.Entities;

public abstract class FinanceInstrument : AnalyticalEntity
{
    public int Id { get; set; }
    public int PlatformId { get; set; }
    public string Ticker { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public Status Status { get; set; }

    public virtual FinancePlatform Platform { get; set; } = default!;
}
