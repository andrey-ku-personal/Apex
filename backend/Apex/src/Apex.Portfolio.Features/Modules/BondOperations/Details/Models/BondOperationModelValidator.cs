using FluentValidation;

namespace Apex.Portfolio.Features.Modules.BondOperations.Details.Models;

public class BondOperationModelValidator : AbstractValidator<BondOperationModel>
{
    public BondOperationModelValidator()
    {
        RuleFor(x => x.BondId).GreaterThan(0);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Date).NotEmpty()
            .LessThanOrEqualTo(_ => DateOnly.FromDateTime(DateTime.UtcNow));
        RuleFor(x => x.Count).GreaterThan(0);
        RuleFor(x => x.Price).GreaterThan(0).PrecisionScale(18, 2, ignoreTrailingZeros: false);
        RuleFor(x => x.Commission).GreaterThanOrEqualTo(0).PrecisionScale(18, 2, ignoreTrailingZeros: false);
    }
}
