using Apex.Invest.Features.Modules.Shares.List.Filters;
using Apex.Invest.Features.Modules.Shares.List.Models;
using Apex.Invest.Features.Modules.Shares.List.Services;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Core.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Apex.Invest.Features.Modules.Shares.List.Endpoints;

public class ShareListController(IShareListService listService) : SharedController
{
    [HttpPost("Share/Get/List")]
    [OpenApiOperation(
        operationId: "Share.Get/List",
        summary: "Get List Shares",
        description: "Get paginated list of Shares")
    ]
    [OpenApiTags("Share")]
    public async Task<PageDataResponse<ShareListModel>> GetList([FromBody] ShareListFilter filter, CancellationToken cancellationToken = default)
        => await listService.GetList(filter, cancellationToken);
}
