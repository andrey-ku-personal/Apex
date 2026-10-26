using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Filtering;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Models;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Services;
using Apex.Shared.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Endpoints;

public class InstrumentMetadataController(IInstrumentMetadataService service) : SharedController
{
    [HttpGet("InstrumentMetadata/Get/{id:int}")]
    [OpenApiOperation(
        operationId: "InstrumentMetadata.Get",
        summary: "Get InstrumentMetadata",
        description: "Get InstrumentMetadata by id")
    ]
    [OpenApiTags("InstrumentMetadata")]
    public async Task<InstrumentMetadataModel> Get([FromRoute] InstrumentMetadataFilter filter, CancellationToken cancellationToken = default)
        => await service.Get(filter, cancellationToken);

    [HttpPost("InstrumentMetadata/Create")]
    [OpenApiOperation(
        operationId: "InstrumentMetadata.Create",
        summary: "Create InstrumentMetadata",
        description: "Create new InstrumentMetadata (Currency or PaymentFrequency)")
    ]
    [OpenApiTags("InstrumentMetadata")]
    public async Task<InstrumentMetadataModel> Create([FromBody] InstrumentMetadataModel model, CancellationToken cancellationToken = default)
        => await service.Create(model, cancellationToken);

    [HttpPut("InstrumentMetadata/Update/{id:int}")]
    [OpenApiOperation(
        operationId: "InstrumentMetadata.Update",
        summary: "Update InstrumentMetadata",
        description: "Update InstrumentMetadata name by id (Type is immutable)")
    ]
    [OpenApiTags("InstrumentMetadata")]
    public async Task<InstrumentMetadataModel> Update([FromRoute] InstrumentMetadataFilter filter, [FromBody] InstrumentMetadataModel model, CancellationToken cancellationToken = default)
        => await service.Update(filter, model, cancellationToken);

    [HttpDelete("InstrumentMetadata/Delete/{id:int}")]
    [OpenApiOperation(
        operationId: "InstrumentMetadata.Delete",
        summary: "Delete InstrumentMetadata",
        description: "Delete InstrumentMetadata by id if it is not used by bonds")
    ]
    [OpenApiTags("InstrumentMetadata")]
    public async Task Delete([FromRoute] InstrumentMetadataFilter filter, CancellationToken cancellationToken = default)
        => await service.Delete(filter, cancellationToken);
}