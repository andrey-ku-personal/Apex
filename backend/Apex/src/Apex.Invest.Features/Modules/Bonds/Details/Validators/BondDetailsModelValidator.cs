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
        RuleFor(x => x.Currency).IsInEnum();
        RuleFor(x => x.PaymentFrequence).IsInEnum();
        RuleFor(x => x.ParPrice).GreaterThan(0);
        RuleFor(x => x.CouponRate).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MaturityDate).GreaterThan(DateTime.MinValue);
        RuleFor(x => x.NextCouponDate).GreaterThan(0);
    }
}