using Apex.Invest.Features.Modules.Bonds.Details.Filters;
using Apex.Invest.Features.Modules.Bonds.Details.Models;
using Apex.Invest.Features.Modules.Bonds.Details.Services;
using Apex.Shared.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Apex.Invest.Features.Modules.Bonds.Details.Endpoints;

public class BondDetailsController(IBondDetailsService service) : SharedController
{
    [HttpPost("Bond/Details")]
    [OpenApiOperation(
        operationId: "Bond.Save",
        summary: "Save Bond",
        description: "Save Bond")
    ]
    [OpenApiTags("Bond")]
    public async Task<BondDetailsModel> Save([FromBody] BondDetailsModel model, CancellationToken cancellationToken = default)
        => await service.Upsert(model, cancellationToken);

    [HttpGet("Bond/Details/{id:int}")]
    [OpenApiOperation(
        operationId: "Bond.Get",
        summary: "Get Bond",
        description: "Get Bond")
    ]
    [OpenApiTags("Bond")]
    public async Task<BondDetailsModel> Get([FromRoute] BondDetailsFilter filter, CancellationToken cancellationToken = default)
        => await service.Get(filter, cancellationToken);
}
