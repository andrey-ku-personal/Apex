using FluentValidation;
using Apex.Invest.Features.Modules.Shares.Details.Models;

namespace Apex.Invest.Features.Modules.Shares.Details.Validators;

public class ShareDetailsModelValidator : AbstractValidator<ShareDetailsModel>
{
    public ShareDetailsModelValidator()
    {
        RuleFor(x => x.Ticker).NotNull().NotEmpty();
        RuleFor(x => x.Issuer).NotNull().NotEmpty();
        RuleFor(x => x.PlatformId).GreaterThan(0);
        RuleFor(x => x.MarketPrice).GreaterThan(0);
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.DividendRate).GreaterThanOrEqualTo(0);
    }
}
