using Apex.Invest.Abstracts.Details;
using Apex.Invest.Persistance;
using Apex.Shared.Core.Exceptions;
using Apex.Shared.Core.Pagination.Abstract;
using Apex.Shared.Core.Pagination.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Apex.Invest.Modules;

public abstract class BaseManageService<TModel, TEntity, TFilter, TQuery>(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IBaseDetailsMapper<TModel, TEntity, TFilter, TQuery> mapper
) : IBaseDetailsService<TModel, TEntity, TFilter>
    where TModel : class
    where TEntity : class, new()
    where TFilter : class
    where TQuery : IBaseQuery<TEntity>
{
    public IBaseDetailsMapper<TModel, TEntity, TFilter, TQuery> Mapper => mapper;

    protected virtual string GetNotFoundMessage(object? context) => $"{typeof(TEntity)} was not found";

    public virtual async Task<TModel> Upsert(TModel model, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        await db.BeginTransactionAsync();

        var entity = await UpsertData(db, model, cancellationToken);

        await db.CommitTransactionAsync();

        return await Get(mapper.MapToFilter(entity), cancellationToken);
    }

    protected abstract Task<TEntity> UpsertData(EntitiesDbContext db, TModel model, CancellationToken cancellationToken);

    public async Task<TModel> Get(TFilter filter, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var entity = await db.Set<TEntity>()
            .AsNoTracking()
            .ByQuery(mapper.MapToQuery(filter))
            .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException(GetNotFoundMessage(filter));

        return mapper.MapToModel(entity);
    }
}
