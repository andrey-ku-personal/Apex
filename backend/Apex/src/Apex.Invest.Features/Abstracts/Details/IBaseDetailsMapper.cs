using Apex.Shared.Core.Pagination.Abstract;

namespace Apex.Invest.Abstracts.Details;

public interface IBaseDetailsMapper<TModel, TEntity, TFilter, TQuery>
    where TModel : class
    where TEntity : class
    where TFilter : class
    where TQuery : IBaseQuery<TEntity>
{
    public TModel MapToModel(TEntity entity);
    public TFilter MapToFilter(TEntity entity);
    public TQuery MapToQuery(TFilter filter);
    TEntity MapData(TModel model, TEntity entity);
}
