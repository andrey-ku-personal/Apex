using FluentValidation;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Models;

public class InstrumentMetadataModelValidator : AbstractValidator<InstrumentMetadataModel>
{
    public InstrumentMetadataModelValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(128);
        RuleFor(x => x.Type).IsInEnum();
    }
}
