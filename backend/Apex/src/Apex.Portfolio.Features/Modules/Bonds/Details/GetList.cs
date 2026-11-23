using System.Linq.Expressions;
using System.Security.Claims;
using Apex.Portfolio.Factories;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Core.Pagination;
using Apex.Shared.Core.Pagination.Models;
using Apex.Shared.Core.Services;
using Apex.Shared.Models.Pagination;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using Riok.Mapperly.Abstractions;
using BondEntity = Apex.Portfolio.Domain.Entities.Bond.Bond;

namespace Apex.Portfolio.Features.Modules.Bonds.Details;

public class GetListFilter : PageFilter
{
    public string UserId { get; set; } = null!;
    public int? CurrencyId { get; set; }
    public int? PaymentFrequencyId { get; set; }
}

public class GetListFilterValidator : AbstractValidator<GetListFilter>
{
    public GetListFilterValidator()
    {
        RuleFor(x => x.UserId).NotNull().NotEmpty().MaximumLength(64);
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.FreeText).MaximumLength(64);
    }
}

public class BondListModel
{
    public int Id { get; set; }
    public string Ticker { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public int CurrencyId { get; set; }
    public int PaymentFrequencyId { get; set; }
    public decimal Rate { get; set; }
    public DateOnly MaturityDate { get; set; }
}

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class BondListMapper
{
#pragma warning disable RMG020
#pragma warning disable RMG012

    public partial List<BondListModel> MapToModel(List<BondEntity> entities);
    public partial BondListQuery MapToQuery(GetListFilter filter);

#pragma warning restore RMG020
#pragma warning restore RMG012
}

public class GetListHandler(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory,
    IValidationRunner validation)
    : GetListHandler<BondListModel, GetListFilter, BondEntity, BondListQuery>(dbFactory, validation)
{
    private readonly BondListMapper mapper = mapperFactory.GetMapper<BondListMapper>();

    protected override BondListQuery MapToQuery(GetListFilter filter)
        => mapper.MapToQuery(filter);

    protected override List<BondListModel> MapToModel(List<BondEntity> entities)
        => mapper.MapToModel(entities);
}

[Authorize]
public class GetListController(GetListHandler handler) : SharedController
{
    [HttpPost("Bond/Get/List")]
    [OpenApiOperation(
        operationId: "Bond.Get/List",
        summary: "Get List Bonds",
        description: "Get paginated list of own Bonds")]
    [OpenApiTags("Bond")]
    public async Task<PageDataResponse<BondListModel>> GetList([FromBody] GetListFilter filter, CancellationToken cancellationToken = default)
    {
        filter.UserId = User.FindFirstValue("sub")!;
        return await handler.Handle(filter, cancellationToken);
    }
}

public class BondListQuery : BaseSortQuery<BondEntity>
{
    public string UserId { get; set; } = null!;
    public int? CurrencyId { get; set; }
    public int? PaymentFrequencyId { get; set; }
    public string? FreeText { get; set; }

    public override Expression<Func<BondEntity, bool>> GetExpression()
        => bond =>
            bond.UserId == UserId &&
            (CurrencyId == null || bond.CurrencyId == CurrencyId) &&
            (PaymentFrequencyId == null || bond.PaymentFrequencyId == PaymentFrequencyId) &&
            (string.IsNullOrWhiteSpace(FreeText) ||
             EF.Functions.ILike(bond.Ticker, $"%{FreeText}%") ||
             EF.Functions.ILike(bond.Issuer, $"%{FreeText}%"));
}
