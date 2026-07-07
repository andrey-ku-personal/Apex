using Microsoft.EntityFrameworkCore;
using Apex.Shared.Core.Pagination.Models;

namespace Apex.Shared.Core.Pagination.Expressions;

public static class PaginationExtension
{
    public static PageDataResponse<TEntity> PageResult<TEntity>(this IQueryable<TEntity> query, int skipPages, int pageSize)
        where TEntity : class
    {
        var count = query.Count();

        var result = query
            .Skip(skipPages)
            .Take(pageSize == 0 ? count : pageSize)
            .ToList();

        return new PageDataResponse<TEntity>(count, result);
    }

    public static async Task<PageDataResponse<TEntity>> PageResultAsync<TEntity>(this IQueryable<TEntity> query, int skip, int take)
        where TEntity : class
    {
        var count = query.Count();

        var result = await query.Skip(skip).Take(take).ToListAsync();

        return new PageDataResponse<TEntity>(count, result);
    }
}