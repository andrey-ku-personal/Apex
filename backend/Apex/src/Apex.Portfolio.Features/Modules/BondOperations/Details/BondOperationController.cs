using System.Security.Claims;
using Apex.Portfolio.Features.Modules.BondOperations.Details.Filtering;
using Apex.Portfolio.Features.Modules.BondOperations.Details.Models;
using Apex.Portfolio.Features.Modules.BondOperations.Details.Services;
using Apex.Shared.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Apex.Portfolio.Features.Modules.BondOperations.Details;

[Authorize]
public class BondOperationController(IBondOperationService service) : SharedController
{
    [HttpGet("BondOperation/Get/{id:int}")]
    [OpenApiOperation(
        operationId: "BondOperation.Get",
        summary: "Get BondOperation",
        description: "Get own BondOperation by id")]
    [OpenApiTags("BondOperation")]
    public async Task<BondOperationModel> Get([FromRoute] BondOperationFilter filter, CancellationToken cancellationToken = default)
    {
        filter.UserId = User.FindFirstValue("sub")!;
        return await service.Get(filter, cancellationToken);
    }

    [HttpPost("BondOperation/Create")]
    [OpenApiOperation(
        operationId: "BondOperation.Create",
        summary: "Create BondOperation",
        description: "Create buy or sell operation for the current user")]
    [OpenApiTags("BondOperation")]
    public async Task<BondOperationModel> Create([FromBody] BondOperationModel model, CancellationToken cancellationToken = default)
    {
        model.UserId = User.FindFirstValue("sub")!;
        return await service.Create(model, cancellationToken);
    }

    [HttpPut("BondOperation/Update/{id:int}")]
    [OpenApiOperation(
        operationId: "BondOperation.Update",
        summary: "Update BondOperation",
        description: "Update own operation by id (position cannot go negative)")]
    [OpenApiTags("BondOperation")]
    public async Task<BondOperationModel> Update([FromRoute] BondOperationFilter filter, [FromBody] BondOperationModel model, CancellationToken cancellationToken = default)
    {
        filter.UserId = User.FindFirstValue("sub")!;
        model.UserId = filter.UserId;
        return await service.Update(filter, model, cancellationToken);
    }

    [HttpDelete("BondOperation/Delete/{id:int}")]
    [OpenApiOperation(
        operationId: "BondOperation.Delete",
        summary: "Delete BondOperation",
        description: "Delete own operation by id to fix a mistake (position cannot go negative)")]
    [OpenApiTags("BondOperation")]
    public async Task Delete([FromRoute] BondOperationFilter filter, CancellationToken cancellationToken = default)
    {
        filter.UserId = User.FindFirstValue("sub")!;
        await service.Delete(filter, cancellationToken);
    }
}
