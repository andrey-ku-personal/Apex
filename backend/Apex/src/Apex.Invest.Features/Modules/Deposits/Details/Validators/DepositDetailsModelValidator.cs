using FluentValidation;
using Apex.Invest.Features.Modules.Deposits.Details.Models;

namespace Apex.Invest.Features.Modules.Deposits.Details.Validators;

public class DepositDetailsModelValidator : AbstractValidator<DepositDetailsModel>
{
    public DepositDetailsModelValidator()
    {
        RuleFor(x => x.Ticker).NotNull().NotEmpty();
        RuleFor(x => x.Issuer).NotNull().NotEmpty();
        RuleFor(x => x.PlatformId).GreaterThan(0);
        RuleFor(x => x.InterestRate).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MaturityDate).GreaterThan(x => x.FirstPaymentDate);
    }
}
