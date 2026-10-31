using Apex.Portfolio.Modules;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Filtering;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Models;
using Apex.Shared.Core.Services;
using Apex.Shared.Models.Pagination;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Services;

public class InstrumentMetadataListDecorator(
    InstrumentMetadataListService instrumentMetadataListService,
    IValidationRunner validationRunner
) : IInstrumentMetadataListService
{
    private readonly BaseGetListServiceDecorator<InstrumentMetadataListModel, Domain.Abstractions.InstrumentMetadata, InstrumentMetadataListFilter, InstrumentMetadataListQuery> _decorator =
        new(instrumentMetadataListService, validationRunner);

    public Task<PageDataResponse<InstrumentMetadataListModel>> GetList(InstrumentMetadataListFilter filter, CancellationToken cancellationToken) =>
        _decorator.GetList(filter, cancellationToken);
}
