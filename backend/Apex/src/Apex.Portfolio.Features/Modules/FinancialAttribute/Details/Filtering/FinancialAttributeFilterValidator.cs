using FluentValidation;

namespace Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Filtering;

public class FinancialAttributeFilterValidator : AbstractValidator<FinancialAttributeFilter>
{
    public FinancialAttributeFilterValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
