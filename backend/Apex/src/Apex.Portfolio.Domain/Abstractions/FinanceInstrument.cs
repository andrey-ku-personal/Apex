namespace Apex.Portfolio.Domain.Abstractions;

public abstract class FinanceInstrument : AnalyticalEntity
{
    public int Id { get; set; }
    public int PlatformId { get; set; }
    public string Ticker { get; set; } = default!;
    public string Issuer { get; set; } = default!;
}
