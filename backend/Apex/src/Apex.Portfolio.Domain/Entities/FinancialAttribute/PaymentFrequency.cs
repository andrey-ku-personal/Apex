namespace Apex.Portfolio.Domain.Entities.FinancialAttribute;

public class PaymentFrequency : Abstractions.FinancialAttribute
{
    public PaymentFrequency()
    {
        Type = Enums.MetadataType.PaymentFrequency;
    }
}