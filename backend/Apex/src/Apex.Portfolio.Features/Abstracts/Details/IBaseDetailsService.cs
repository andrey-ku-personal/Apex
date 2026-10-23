namespace Apex.Portfolio.Abstracts.Details;

public interface IBaseDetailsService<TModel, TEntity, TFilter>
    where TModel : class
    where TEntity : class
    where TFilter : class
{
    Task<TModel> Create(TModel model, CancellationToken cancellationToken);
    Task<TModel> Update(TFilter filter, TModel model, CancellationToken cancellationToken);
    Task<TModel> Get(TFilter query, CancellationToken cancellationToken);
    Task Delete(TFilter filter, CancellationToken cancellationToken);
}
