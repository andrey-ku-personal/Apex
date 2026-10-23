using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Filtering;
using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Models;
using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Services;
using Apex.Shared.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Endpoints;

public class FinancialAttributeController(IFinancialAttributeService service) : SharedController
{
    [HttpGet("FinancialAttribute/Get/{id:int}")]
    [OpenApiOperation(
        operationId: "FinancialAttribute.Get",
        summary: "Get FinancialAttribute",
        description: "Get FinancialAttribute by id")
    ]
    [OpenApiTags("FinancialAttribute")]
    public async Task<FinancialAttributeModel> Get([FromRoute] FinancialAttributeFilter filter, CancellationToken cancellationToken = default)
        => await service.Get(filter, cancellationToken);

    [HttpPost("FinancialAttribute/Create")]
    [OpenApiOperation(
        operationId: "FinancialAttribute.Create",
        summary: "Create FinancialAttribute",
        description: "Create new FinancialAttribute (Currency or PaymentFrequency)")
    ]
    [OpenApiTags("FinancialAttribute")]
    public async Task<FinancialAttributeModel> Create([FromBody] FinancialAttributeModel model, CancellationToken cancellationToken = default)
        => await service.Create(model, cancellationToken);

    [HttpPut("FinancialAttribute/Update/{id:int}")]
    [OpenApiOperation(
        operationId: "FinancialAttribute.Update",
        summary: "Update FinancialAttribute",
        description: "Update FinancialAttribute name by id (Type is immutable)")
    ]
    [OpenApiTags("FinancialAttribute")]
    public async Task<FinancialAttributeModel> Update([FromRoute] FinancialAttributeFilter filter, [FromBody] FinancialAttributeModel model, CancellationToken cancellationToken = default)
        => await service.Update(filter, model, cancellationToken);

    [HttpDelete("FinancialAttribute/Delete/{id:int}")]
    [OpenApiOperation(
        operationId: "FinancialAttribute.Delete",
        summary: "Delete FinancialAttribute",
        description: "Delete FinancialAttribute by id if it is not used by bonds")
    ]
    [OpenApiTags("FinancialAttribute")]
    public async Task Delete([FromRoute] FinancialAttributeFilter filter, CancellationToken cancellationToken = default)
        => await service.Delete(filter, cancellationToken);
}
