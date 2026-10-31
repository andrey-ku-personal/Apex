using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Filtering;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Models;
using Apex.Portfolio.Modules;
using Apex.Shared.Core.Services;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Services;

public class InstrumentMetadataDecorator(
    InstrumentMetadataService service,
    IValidationRunner validator
) : BaseManageServiceDecorator<
        InstrumentMetadataModel, 
        Domain.Abstractions.InstrumentMetadata, 
        InstrumentMetadataFilter, 
        InstrumentMetadataQuery
    >(service, validator), IInstrumentMetadataService;