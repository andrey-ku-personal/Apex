using Apex.Invest.Features.Modules.Deposits.Details.Filters;
using Apex.Invest.Features.Modules.Deposits.Details.Models;
using Apex.Invest.Features.Modules.Deposits.Details.Services;
using Apex.Shared.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Apex.Invest.Features.Modules.Deposits.Details.Endpoints;

public class DepositDetailsController(IDepositDetailsService service) : SharedController
{
    [HttpPut("Deposit/Details/Upsert")]
    [OpenApiOperation(
        operationId: "Deposit.Upsert",
        summary: "Upsert Deposit",
        description: "Upsert Deposit")
    ]
    [OpenApiTags("Deposit")]
    public async Task<DepositDetailsModel> Upsert([FromBody] DepositDetailsModel model, CancellationToken cancellationToken = default)
        => await service.Upsert(model, cancellationToken);

    [HttpGet("Deposit/Details/{id:int}")]
    [OpenApiOperation(
        operationId: "Deposit.Get",
        summary: "Get Deposit",
        description: "Get Deposit")
    ]
    [OpenApiTags("Deposit")]
    public async Task<DepositDetailsModel> Get([FromRoute] DepositDetailsFilter filter, CancellationToken cancellationToken = default)
        => await service.Get(filter, cancellationToken);
}
