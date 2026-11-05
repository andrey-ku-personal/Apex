using System.Linq.Expressions;
using Apex.Shared.Core.Pagination.Abstract;

namespace Apex.Shared.Core.Pagination;

public class BaseQuery<TEntity> : IBaseQuery<TEntity>
    where TEntity : class
{
    public virtual Expression<Func<TEntity, bool>> GetExpression()
    {
        Expression<Func<TEntity, bool>> filter = uniqueEntity => true;
        return filter;
    }

    public virtual List<Expression<Func<TEntity, object>>> GetIncludes()
    {
        return [];
    }
}