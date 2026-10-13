namespace Apex.Portfolio.Domain.Entities.FinancialAttribute;

public class Currency : Abstractions.FinancialAttribute
{
    public Currency()
    {
        Type = Enums.MetadataType.Currency;
    }
}