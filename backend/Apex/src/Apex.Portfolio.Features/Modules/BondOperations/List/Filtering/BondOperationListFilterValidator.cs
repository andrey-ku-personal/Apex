using FluentValidation;

namespace Apex.Portfolio.Features.Modules.BondOperations.List.Filtering;

public class BondOperationListFilterValidator : AbstractValidator<BondOperationListFilter>
{
    public BondOperationListFilterValidator()
    {
        RuleFor(x => x.UserId).NotNull().NotEmpty().MaximumLength(64);
        RuleFor(x => x.BondId).GreaterThan(0).When(x => x.BondId.HasValue);
        RuleFor(x => x.Type).IsInEnum().When(x => x.Type.HasValue);
        RuleFor(x => x.DateTo).GreaterThanOrEqualTo(x => x.DateFrom!.Value).When(x => x.DateFrom.HasValue && x.DateTo.HasValue);
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
