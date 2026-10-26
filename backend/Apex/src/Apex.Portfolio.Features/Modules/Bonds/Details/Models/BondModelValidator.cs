using FluentValidation;

namespace Apex.Portfolio.Features.Modules.Bonds.Details.Models;

public class BondModelValidator : AbstractValidator<BondModel>
{
    public BondModelValidator()
    {
        RuleFor(x => x.Ticker).NotNull().NotEmpty();
        RuleFor(x => x.Issuer).NotNull().NotEmpty();
        RuleFor(x => x.CurrencyId).GreaterThan(0);
        RuleFor(x => x.PaymentFrequencyId).GreaterThan(0);
        RuleFor(x => x.PaymentDay).InclusiveBetween(1, 31);
        RuleFor(x => x.Rate).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ParPrice).GreaterThan(0);
        RuleFor(x => x.MaturityDate).GreaterThan(x => x.IssueDate);
    }
}
