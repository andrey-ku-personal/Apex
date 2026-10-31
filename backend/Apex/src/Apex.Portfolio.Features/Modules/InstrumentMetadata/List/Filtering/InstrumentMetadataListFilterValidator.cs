using FluentValidation;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Filtering;

public class InstrumentMetadataListFilterValidator : AbstractValidator<InstrumentMetadataListFilter>
{
    public InstrumentMetadataListFilterValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.FreeText).MaximumLength(128);
    }
}
