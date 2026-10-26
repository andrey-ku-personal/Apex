namespace Apex.Portfolio.Domain.Entities.InstrumentMetadata;

public class Currency : Abstractions.InstrumentMetadata
{
    public Currency()
    {
        Type = Enums.MetadataType.Currency;
    }
}