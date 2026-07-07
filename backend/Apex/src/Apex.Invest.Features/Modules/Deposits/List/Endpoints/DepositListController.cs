using Apex.Invest.Features.Modules.Deposits.List.Filters;
using Apex.Invest.Features.Modules.Deposits.List.Models;
using Apex.Invest.Features.Modules.Deposits.List.Services;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Core.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Apex.Invest.Features.Modules.Deposits.List.Endpoints;

public class DepositListController(IDepositListService listService) : SharedController
{
    [HttpPost("Deposit/Get/List")]
    [OpenApiOperation(
        operationId: "Deposit.Get/List",
        summary: "Get List Deposits",
        description: "Get paginated list of Deposits")
    ]
    [OpenApiTags("Deposit")]
    public async Task<PageDataResponse<DepositListModel>> GetList([FromBody] DepositListFilter filter, CancellationToken cancellationToken = default)
        => await listService.GetList(filter, cancellationToken);
}
