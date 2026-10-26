namespace Apex.Portfolio.Domain.Entities.InstrumentMetadata;

public class PaymentFrequency : Abstractions.InstrumentMetadata
{
    public PaymentFrequency()
    {
        Type = Enums.MetadataType.PaymentFrequency;
    }
}