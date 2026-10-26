using Apex.Portfolio.Abstracts.List;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Filtering;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Models;
namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Services;

public interface IInstrumentMetadataListService : IBaseListService<InstrumentMetadataListModel, Domain.Abstractions.InstrumentMetadata, InstrumentMetadataListFilter, InstrumentMetadataListQuery>;