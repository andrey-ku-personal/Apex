using System.Linq.Expressions;

namespace Apex.Shared.Core.Pagination.Abstract;

public interface IBaseSortQuery<TEntity> : IBaseQuery<TEntity>
    where TEntity : class
{
    public string SortBy { get; set; }
    public bool IsAscending { get; set; }

    public abstract Expression<Func<TEntity, object>> GetSortingExpression();
}