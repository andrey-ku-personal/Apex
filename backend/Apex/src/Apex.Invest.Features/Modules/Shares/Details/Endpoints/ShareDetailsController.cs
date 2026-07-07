using Apex.Invest.Features.Modules.Shares.Details.Filters;
using Apex.Invest.Features.Modules.Shares.Details.Models;
using Apex.Invest.Features.Modules.Shares.Details.Services;
using Apex.Shared.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Apex.Invest.Features.Modules.Shares.Details.Endpoints;

public class ShareDetailsController(IShareDetailsService service) : SharedController
{
    [HttpPut("Share/Details/Upsert")]
    [OpenApiOperation(
        operationId: "Share.Upsert",
        summary: "Upsert Share",
        description: "Upsert Share")
    ]
    [OpenApiTags("Share")]
    public async Task<ShareDetailsModel> Upsert([FromBody] ShareDetailsModel model, CancellationToken cancellationToken = default)
        => await service.Upsert(model, cancellationToken);

    [HttpGet("Share/Details/{id:int}")]
    [OpenApiOperation(
        operationId: "Share.Get",
        summary: "Get Share",
        description: "Get Share")
    ]
    [OpenApiTags("Share")]
    public async Task<ShareDetailsModel> Get([FromRoute] ShareDetailsFilter filter, CancellationToken cancellationToken = default)
        => await service.Get(filter, cancellationToken);
}
