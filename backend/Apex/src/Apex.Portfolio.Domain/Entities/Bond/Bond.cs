using Apex.Portfolio.Domain.Abstractions;
using Apex.Portfolio.Domain.Entities.InstrumentMetadata;

namespace Apex.Portfolio.Domain.Entities.Bond;

public class Bond : FinanceInstrument
{
    public int CurrencyId { get; set; }
    public int PaymentFrequencyId { get; set; }

    public DateOnly IssueDate { get; set; }
    public DateOnly MaturityDate { get; set; }
    public int PaymentDay { get; set; }

    public decimal ParPrice { get; set; }
    public decimal Rate { get; set; }
    public bool IsRefinancingLinked { get; set; }

    public Currency Currency { get; set; } = default!;
    public PaymentFrequency PaymentFrequency { get; set; } = default!;
}