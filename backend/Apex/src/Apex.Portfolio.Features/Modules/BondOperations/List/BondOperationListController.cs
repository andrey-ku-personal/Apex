using System.Security.Claims;
using Apex.Portfolio.Features.Modules.BondOperations.List.Filtering;
using Apex.Portfolio.Features.Modules.BondOperations.List.Models;
using Apex.Portfolio.Features.Modules.BondOperations.List.Services;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Models.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Apex.Portfolio.Features.Modules.BondOperations.List;

[Authorize]
public class BondOperationListController(IBondOperationListService listService) : SharedController
{
    [HttpPost("BondOperation/Get/List")]
    [OpenApiOperation(
        operationId: "BondOperation.Get/List",
        summary: "Get List BondOperations",
        description: "Get paginated history of own operations, filterable by bond, type and date range")]
    [OpenApiTags("BondOperation")]
    public async Task<PageDataResponse<BondOperationListModel>> GetList([FromBody] BondOperationListFilter filter, CancellationToken cancellationToken = default)
    {
        filter.UserId = User.FindFirstValue("sub")!;
        return await listService.GetList(filter, cancellationToken);
    }
}
