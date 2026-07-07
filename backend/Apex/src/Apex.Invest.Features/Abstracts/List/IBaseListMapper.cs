using Apex.Shared.Core.Pagination.Abstract;

namespace Apex.Invest.Abstracts.List;

public interface IBaseListMapper<TModel, TEntity, TFilter, TQuery>
    where TModel : class
    where TEntity : class
    where TFilter : IPageFilter
    where TQuery : IBaseQuery<TEntity>
{
    public List<TModel> MapToModel(List<TEntity> entity);
    public TQuery MapToQuery(TFilter filter);
}
