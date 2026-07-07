using FluentValidation;
using Apex.Invest.Features.Modules.Shares.Details.Filters;

namespace Apex.Invest.Features.Modules.Shares.Details.Validators;

public class ShareDetailsFilterValidator : AbstractValidator<ShareDetailsFilter>
{
    public ShareDetailsFilterValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
