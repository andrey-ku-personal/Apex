using Apex.Portfolio.Features.Modules.FinancialAttribute.List.Filters;
using Apex.Portfolio.Features.Modules.FinancialAttribute.List.Models;
using Apex.Portfolio.Features.Modules.FinancialAttribute.List.Services;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Models.Pagination;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Apex.Portfolio.Features.Modules.FinancialAttribute.List.Endpoints;

public class FinancialAttributeListController(IFinancialAttributeListService listService) : SharedController
{
    [HttpPost("FinancialAttribute/Get/List")]
    [OpenApiOperation(
        operationId: "FinancialAttribute.Get/List",
        summary: "Get List FinancialAttributes",
        description: "Get paginated list of FinancialAttributes (Currency, PaymentFrequency)")
    ]
    [OpenApiTags("FinancialAttribute")]
    public async Task<PageDataResponse<FinancialAttributeListModel>> GetList([FromBody] FinancialAttributeListFilter filter, CancellationToken cancellationToken = default)
        => await listService.GetList(filter, cancellationToken);
}
