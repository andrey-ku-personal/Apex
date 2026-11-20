using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Pagination;
using Apex.Shared.Core.Pagination.Expressions;
using Apex.Shared.Core.Pagination.Models;
using Apex.Shared.Core.Services;
using Apex.Shared.Models.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Apex.Portfolio.Features.Modules;

public abstract class GetListHandler<TModel, TFilter, TEntity, TQuery>(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IValidationRunner validation)
    where TModel : class
    where TFilter : PageFilter
    where TQuery : BaseSortQuery<TEntity>
    where TEntity : class
{
    protected abstract TQuery MapToQuery(TFilter filter);

    protected abstract List<TModel> MapToModel(List<TEntity> entities);

    public async Task<PageDataResponse<TModel>> Handle(TFilter filter, CancellationToken cancellationToken)
    {
        await validation.ValidateAsync(filter, cancellationToken);

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var (count, items) = await db.Set<TEntity>().AsNoTracking()
            .ByQuery(MapToQuery(filter))
            .PaginateAsync(filter, cancellationToken);

        return new PageDataResponse<TModel>(count, MapToModel(items));
    }
}
