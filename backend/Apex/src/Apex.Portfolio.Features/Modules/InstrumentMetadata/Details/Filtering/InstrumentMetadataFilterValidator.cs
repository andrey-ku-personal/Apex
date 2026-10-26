using FluentValidation;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Filtering;

public class InstrumentMetadataFilterValidator : AbstractValidator<InstrumentMetadataFilter>
{
    public InstrumentMetadataFilterValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
