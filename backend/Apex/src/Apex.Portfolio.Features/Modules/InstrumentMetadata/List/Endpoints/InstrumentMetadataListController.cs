using Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Filtering;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Models;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Services;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Models.Pagination;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Endpoints;

public class InstrumentMetadataListController(IInstrumentMetadataListService listService) : SharedController
{
    [HttpPost("InstrumentMetadata/Get/List")]
    [OpenApiOperation(
        operationId: "InstrumentMetadata.Get/List",
        summary: "Get List InstrumentMetadata",
        description: "Get paginated list of InstrumentMetadata (Currency, PaymentFrequency)")
    ]
    [OpenApiTags("InstrumentMetadata")]
    public async Task<PageDataResponse<InstrumentMetadataListModel>> GetList([FromBody] InstrumentMetadataListFilter filter, CancellationToken cancellationToken = default)
        => await listService.GetList(filter, cancellationToken);
}