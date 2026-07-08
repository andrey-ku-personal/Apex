using Apex.Shared.Core.Pagination.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Apex.Shared.Core.Pagination.Expressions;

public static class QueryExtension
{
    public static IQueryable<TEntity> ByQuery<TEntity>(this IQueryable<TEntity> items, IBaseQuery<TEntity> query)
        where TEntity : class
    {
        var result = items.Where(query.GetExpression());
        result = query.GetIncludes().Aggregate(result, (current, include) => current.Include(include));
        return result;
    }

    public static IOrderedQueryable<TEntity> ByQuery<TEntity>(this IQueryable<TEntity> items, IBaseSortQuery<TEntity> query)
        where TEntity : class
    {
        var result = items.Where(query.GetExpression());
        result = query.GetIncludes().Aggregate(result, (current, include) => current.Include(include));
        return query.IsAscending
            ? result.OrderBy(query.GetSortingExpression())
            : result.OrderByDescending(query.GetSortingExpression());
    }

    public static (int count, List<TEntity> items) Paginate<TEntity>(this IQueryable<TEntity> items, IPageFilter filter)
        where TEntity : class
        => new(items.Count(), [.. items.Skip(filter.PageNumber * filter.PageSize).Take(filter.PageSize == 0 ? items.Count() : filter.PageSize)]);

    public static async Task<(int count, List<TEntity> items)> PaginateAsync<TEntity>(
        this IQueryable<TEntity> items, IPageFilter filter, CancellationToken cancellationToken)
        where TEntity : class
    {
        var count = await items.CountAsync(cancellationToken);
        var page = await items.Skip(filter.PageNumber * filter.PageSize).Take(filter.PageSize == 0 ? count : filter.PageSize).ToListAsync(cancellationToken);
        return (count, page);
    }
}