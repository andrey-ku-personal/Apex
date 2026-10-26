using Apex.Portfolio.Abstracts.Details;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Filtering;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Models;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Services;

public interface IInstrumentMetadataService : IBaseDetailsService<InstrumentMetadataModel, Domain.Abstractions.InstrumentMetadata, InstrumentMetadataFilter>;