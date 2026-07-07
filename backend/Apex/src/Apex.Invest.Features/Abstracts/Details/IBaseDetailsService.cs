namespace Apex.Invest.Abstracts.Details;

public interface IBaseDetailsService<TModel, TEntity, TFilter>
    where TModel : class
    where TEntity : class, new()
    where TFilter : class
{
    Task<TModel> Upsert(TModel model, CancellationToken cancellationToken);
    Task<TModel> Get(TFilter query, CancellationToken cancellationToken);
}
