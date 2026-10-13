using Apex.Portfolio.Domain.Abstractions;
using Apex.Portfolio.Domain.Entities.FinancialAttribute;

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
    public bool IsFloating { get; set; }

    public Currency Currency { get; set; } = default!;
    public PaymentFrequency PaymentFrequency { get; set; } = default!;
}