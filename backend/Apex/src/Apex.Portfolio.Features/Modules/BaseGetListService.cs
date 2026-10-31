using Apex.Portfolio.Abstracts.List;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Pagination.Abstract;
using Apex.Shared.Core.Pagination.Expressions;
using Apex.Shared.Models.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Apex.Portfolio.Modules;

public class BaseGetListService<TModel, TEntity, TFilter, TQuery>(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IBaseListMapper<TModel, TEntity, TFilter, TQuery> mapper
) : IBaseListService<TModel, TEntity, TFilter, TQuery>
    where TModel : class
    where TEntity : class
    where TFilter : IPageSortQuery
    where TQuery : IBaseSortQuery<TEntity>
{
    public virtual async Task<PageDataResponse<TModel>> GetList(TFilter filter, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var query = mapper.MapToQuery(filter);

        var (count, items) = await db.Set<TEntity>()
            .AsNoTracking()
            .ByQuery(query)
            .PaginateAsync(filter, cancellationToken);

        return new PageDataResponse<TModel>(count, mapper.MapToModel(items));
    }
}
