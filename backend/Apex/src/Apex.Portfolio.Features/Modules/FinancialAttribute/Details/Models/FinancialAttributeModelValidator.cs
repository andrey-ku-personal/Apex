using FluentValidation;

namespace Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Models;

public class FinancialAttributeModelValidator : AbstractValidator<FinancialAttributeModel>
{
    public FinancialAttributeModelValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(128);
        RuleFor(x => x.Type).IsInEnum();
    }
}
