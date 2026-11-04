using FluentValidation;

namespace Apex.Portfolio.Features.Modules.BondOperations.Details.Filtering;

public class BondOperationFilterValidator : AbstractValidator<BondOperationFilter>
{
    public BondOperationFilterValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.UserId).NotNull().NotEmpty().MaximumLength(64);
    }
}
