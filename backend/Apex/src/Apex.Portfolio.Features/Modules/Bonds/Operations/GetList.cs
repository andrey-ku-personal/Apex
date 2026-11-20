using System.Linq.Expressions;
using System.Security.Claims;
using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Factories;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Core.Pagination;
using Apex.Shared.Core.Pagination.Expressions;
using Apex.Shared.Core.Pagination.Models;
using Apex.Shared.Core.Services;
using Apex.Shared.Models.Pagination;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using Riok.Mapperly.Abstractions;
using BondOperationEntity = Apex.Portfolio.Domain.Entities.Bond.BondOperation;

namespace Apex.Portfolio.Features.Modules.Bonds.Operations;

public class GetListFilter : PageFilter
{
    public string UserId { get; set; } = null!;
    public int? BondId { get; set; }
    public OperationType? Type { get; set; }
    public DateOnly? DateFrom { get; set; }
    public DateOnly? DateTo { get; set; }
}

public class GetListFilterValidator : AbstractValidator<GetListFilter>
{
    public GetListFilterValidator()
    {
        RuleFor(x => x.UserId).NotNull().NotEmpty().MaximumLength(64);
        RuleFor(x => x.BondId).GreaterThan(0).When(x => x.BondId.HasValue);
        RuleFor(x => x.Type).IsInEnum().When(x => x.Type.HasValue);
        RuleFor(x => x.DateTo).GreaterThanOrEqualTo(x => x.DateFrom!.Value).When(x => x.DateFrom.HasValue && x.DateTo.HasValue);
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}

public class BondOperationListModel
{
    public int Id { get; set; }
    public int BondId { get; set; }
    public OperationType Type { get; set; }
    public DateOnly Date { get; set; }
    public int Count { get; set; }
    public decimal Price { get; set; }
    public decimal Commission { get; set; }
}

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class BondOperationListMapper
{
#pragma warning disable RMG020
#pragma warning disable RMG012

    public partial List<BondOperationListModel> MapToModel(List<BondOperationEntity> entities);
    public partial BondOperationListQuery MapToQuery(GetListFilter filter);

#pragma warning restore RMG020
#pragma warning restore RMG012
}

public class GetListHandler(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory,
    IValidationRunner validation)
    : GetListHandler<BondOperationListModel, GetListFilter, BondOperationEntity, BondOperationListQuery>(dbFactory, validation)
{
    private readonly BondOperationListMapper mapper = mapperFactory.GetMapper<BondOperationListMapper>();

    protected override BondOperationListQuery MapToQuery(GetListFilter filter)
        => mapper.MapToQuery(filter);

    protected override List<BondOperationListModel> MapToModel(List<BondOperationEntity> entities)
        => mapper.MapToModel(entities);
}

[Authorize]
public class GetListController(GetListHandler handler) : SharedController
{
    [HttpPost("BondOperation/Get/List")]
    [OpenApiOperation(
        operationId: "BondOperation.Get/List",
        summary: "Get List BondOperations",
        description: "Get paginated history of own operations, filterable by bond, type and date range")]
    [OpenApiTags("BondOperation")]
    public async Task<PageDataResponse<BondOperationListModel>> GetList([FromBody] GetListFilter filter, CancellationToken cancellationToken = default)
    {
        filter.UserId = User.FindFirstValue("sub")!;
        return await handler.Handle(filter, cancellationToken);
    }
}

public class BondOperationListQuery : BaseSortQuery<BondOperationEntity>
{
    public string UserId { get; set; } = null!;
    public int? BondId { get; set; }
    public OperationType? Type { get; set; }
    public DateOnly? DateFrom { get; set; }
    public DateOnly? DateTo { get; set; }

    public override Expression<Func<BondOperationEntity, bool>> GetExpression()
        => operation =>
            operation.UserId == UserId &&
            (BondId == null || operation.BondId == BondId) &&
            (Type == null || operation.Type == Type) &&
            (DateFrom == null || operation.Date >= DateFrom) &&
            (DateTo == null || operation.Date <= DateTo);
}
