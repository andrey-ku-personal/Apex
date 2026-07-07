using Apex.Invest.Features.Modules.Bonds.List.Filters;
using Apex.Invest.Features.Modules.Bonds.List.Models;
using Apex.Invest.Features.Modules.Bonds.List.Services;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Core.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Apex.Invest.Features.Modules.Bonds.List.Endpoints;

public class BondListController(IBondListService listService) : SharedController
{
    [HttpPost("Bond/Get/List")]
    [OpenApiOperation(
        operationId: "Bond.Get/List",
        summary: "Get List Bonds",
        description: "Get paginated list of Bonds")
    ]
    [OpenApiTags("Bond")]
    public async Task<PageDataResponse<BondListModel>> GetList([FromBody] BondListFilter filter, CancellationToken cancellationToken = default)
        => await listService.GetList(filter, cancellationToken);
}
