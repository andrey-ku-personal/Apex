using FluentValidation;
using Apex.Invest.Features.Modules.Bonds.Details.Filters;

namespace Apex.Invest.Features.Modules.Bonds.Details.Validators;

public class BondDetailsFilterValidator : AbstractValidator<BondDetailsFilter>
{
    public BondDetailsFilterValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
