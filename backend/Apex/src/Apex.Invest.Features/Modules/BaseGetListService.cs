using Apex.Invest.Abstracts.List;
using Apex.Invest.Persistance;
using Apex.Shared.Core.Pagination.Abstract;
using Apex.Shared.Core.Pagination.Expressions;
using Apex.Shared.Core.Pagination.Models;
using Microsoft.EntityFrameworkCore;

namespace Apex.Invest.Modules;

public class BaseGetListService<TModel, TEntity, TFilter, TQuery>(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IBaseListMapper<TModel, TEntity, TFilter, TQuery> mapper
) : IBaseListService<TModel, TEntity, TFilter, TQuery>
    where TModel : class
    where TEntity : class
    where TFilter : IPageSortQuery
    where TQuery : IBaseSortQuery<TEntity>, new()
{
    public async Task<PageDataResponse<TModel>> GetList(TFilter filter, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var (count, items) = db.Set<TEntity>()
            .AsNoTracking()
            .ByQuery(new TQuery() { SortBy = filter.SortBy, IsAscending = filter.IsAscending })
            .Paginate(filter);

        return new PageDataResponse<TModel>(count, mapper.MapToModel(items));
    }
}
