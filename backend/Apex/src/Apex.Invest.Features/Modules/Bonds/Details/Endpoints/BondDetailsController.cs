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
    [OpenApiOperation(operationId: "Bond.Create", summary: "Create Bond", description: "Create Bond")]
    [OpenApiTags("Bond")]
    public async Task<BondDetailsModel> Create([FromBody] BondDetailsModel model, CancellationToken cancellationToken = default)
        => await service.Upsert(model, cancellationToken);

    [HttpPut("Bond/Details/{id:int}")]
    [OpenApiOperation(operationId: "Bond.Update", summary: "Update Bond", description: "Update Bond")]
    [OpenApiTags("Bond")]
    public async Task<BondDetailsModel> Update([FromRoute] int id, [FromBody] BondDetailsModel model, CancellationToken cancellationToken = default)
    {
        model.Id = id;
        return await service.Upsert(model, cancellationToken);
    }

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
