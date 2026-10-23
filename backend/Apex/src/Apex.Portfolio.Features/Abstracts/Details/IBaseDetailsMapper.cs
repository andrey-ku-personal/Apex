using Apex.Shared.Core.Pagination.Abstract;

namespace Apex.Portfolio.Abstracts.Details;

public interface IBaseDetailsMapper<TModel, TEntity, TFilter, TQuery>
    where TModel : class
    where TEntity : class
    where TFilter : class
    where TQuery : IBaseQuery<TEntity>
{
    public TModel MapToModel(TEntity entity);
    public TFilter MapToFilter(TEntity entity);
    public TQuery MapToQuery(TFilter filter);
    TEntity MapToEntity(TModel model);
    TEntity MapData(TModel model, TEntity entity);
}
