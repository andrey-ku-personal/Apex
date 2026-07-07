using Apex.Invest.Abstracts.Details;
using Apex.Invest.Services;
using Apex.Shared.Core.Pagination.Abstract;

namespace Apex.Invest.Modules;

public class BaseManageServiceDecorator<TModel, TEntity, TFilter, TQuery>(
    IBaseDetailsService<TModel, TEntity, TFilter> service,
    IValidationRunner validator
)
    where TModel : class
    where TEntity : class, new()
    where TFilter : class
    where TQuery : IBaseQuery<TEntity>
{
    public async Task<TModel> Upsert(TModel model, CancellationToken cancellationToken)
    {
        await validator.ValidateAsync(model, cancellationToken);
        return await service.Upsert(model, cancellationToken);
    }

    public async Task<TModel> Get(TFilter filter, CancellationToken cancellationToken)
    {
        await validator.ValidateAsync(filter, cancellationToken);
        return await service.Get(filter, cancellationToken);
    }
}
