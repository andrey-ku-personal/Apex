using FluentValidation;
using Apex.Invest.Features.Modules.Deposits.Details.Filters;

namespace Apex.Invest.Features.Modules.Deposits.Details.Validators;

public class DepositDetailsFilterValidator : AbstractValidator<DepositDetailsFilter>
{
    public DepositDetailsFilterValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
