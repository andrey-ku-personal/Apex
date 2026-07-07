using FluentValidation;
using Apex.Invest.Features.Modules.Bonds.Details.Models;

namespace Apex.Invest.Features.Modules.Bonds.Details.Validators;

public class BondDetailsModelValidator : AbstractValidator<BondDetailsModel>
{
    public BondDetailsModelValidator()
    {
        RuleFor(x => x.Ticker).NotNull().NotEmpty();
        RuleFor(x => x.Issuer).NotNull().NotEmpty();
        RuleFor(x => x.PlatformId).GreaterThan(0);
        RuleFor(x => x.InterestRate).GreaterThanOrEqualTo(0);
        RuleFor(x => x.NominalPrice).GreaterThan(0);
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.MaturityDate).GreaterThan(x => x.FirstPaymentDate);
    }
}
