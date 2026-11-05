using System.Linq.Expressions;
using System.Reflection;
using Apex.Shared.Core.Exceptions;
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

        var property = typeof(TEntity).GetProperty(SortBy, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
            ?? throw new BadRequestException($"Cannot sort by '{SortBy}': unknown property of {typeof(TEntity).Name}");

        var expression = Expression.Lambda<Func<TEntity, object>>(Expression.Convert(Expression.Property(parameter, property), typeof(object)), parameter);
        return expression;
    }
}