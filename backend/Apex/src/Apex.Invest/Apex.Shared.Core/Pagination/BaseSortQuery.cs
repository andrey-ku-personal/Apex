using System.Linq.Expressions;
using Apex.Shared.Core.Pagination.Abstract;

namespace Apex.Shared.Core.Pagination;

public class BaseSortQuery<TEntity> : BaseQuery<TEntity>, IBaseSortQuery<TEntity>
    where TEntity : class
{
    public string SortBy { get; set; }
    public bool IsAscending { get; set; }

    protected BaseSortQuery()
    {
        SortBy = "Id";
        IsAscending = false;
    }

    public virtual Expression<Func<TEntity, object>> GetSortingExpression()
    {
        var parameter = Expression.Parameter(typeof(TEntity));
        var property = Expression.Property(parameter, SortBy);

        var expression = Expression.Lambda<Func<TEntity, object>>(Expression.Convert(property, typeof(object)), parameter);
        return expression;
    }
}