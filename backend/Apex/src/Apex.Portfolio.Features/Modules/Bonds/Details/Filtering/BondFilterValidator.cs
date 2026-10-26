using FluentValidation;

namespace Apex.Portfolio.Features.Modules.Bonds.Details.Filtering;

public class BondFilterValidator : AbstractValidator<BondFilter>
{
    public BondFilterValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
