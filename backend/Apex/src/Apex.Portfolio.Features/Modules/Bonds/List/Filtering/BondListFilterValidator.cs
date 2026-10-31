using FluentValidation;

namespace Apex.Portfolio.Features.Modules.Bonds.List.Filtering;

public class BondListFilterValidator : AbstractValidator<BondListFilter>
{
    public BondListFilterValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.FreeText).MaximumLength(64);
    }
}
