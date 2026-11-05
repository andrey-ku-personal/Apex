using System.Linq.Expressions;

namespace Apex.Shared.Core.Pagination.Abstract;

public interface IBaseQuery<TEntity>
    where TEntity : class
{
    public Expression<Func<TEntity, bool>> GetExpression();
    List<Expression<Func<TEntity, object>>> GetIncludes();
}
