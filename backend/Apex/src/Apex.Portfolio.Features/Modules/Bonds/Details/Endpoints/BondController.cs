using Apex.Portfolio.Features.Modules.Bonds.Details.Filtering;
using Apex.Portfolio.Features.Modules.Bonds.Details.Models;
using Apex.Portfolio.Features.Modules.Bonds.Details.Services;
using Apex.Shared.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Apex.Portfolio.Features.Modules.Bonds.Details.Endpoints;

public class BondController(IBondService service) : SharedController
{
    [HttpGet("Bond/Get/{id:int}")]
    [OpenApiOperation(
        operationId: "Bond.Get",
        summary: "Get Bond",
        description: "Get Bond by id")]
    [OpenApiTags("Bond")]
    public async Task<BondModel> Get([FromRoute] BondFilter filter, CancellationToken cancellationToken = default)
        => await service.Get(filter, cancellationToken);

    [HttpPost("Bond/Create")]
    [OpenApiOperation(
        operationId: "Bond.Create",
        summary: "Create Bond",
        description: "Create new Bond in the shared bond library")]
    [OpenApiTags("Bond")]
    public async Task<BondModel> Create([FromBody] BondModel model, CancellationToken cancellationToken = default)
        => await service.Create(model, cancellationToken);

    [HttpPut("Bond/Update/{id:int}")]
    [OpenApiOperation(
        operationId: "Bond.Update",
        summary: "Update Bond",
        description: "Update Bond by id")]
    [OpenApiTags("Bond")]
    public async Task<BondModel> Update([FromRoute] BondFilter filter, [FromBody] BondModel model, CancellationToken cancellationToken = default)
        => await service.Update(filter, model, cancellationToken);

    [HttpDelete("Bond/Delete/{id:int}")]
    [OpenApiOperation(
        operationId: "Bond.Delete",
        summary: "Delete Bond",
        description: "Delete Bond by id")]
    [OpenApiTags("Bond")]
    public async Task Delete([FromRoute] BondFilter filter, CancellationToken cancellationToken = default)
        => await service.Delete(filter, cancellationToken);
}