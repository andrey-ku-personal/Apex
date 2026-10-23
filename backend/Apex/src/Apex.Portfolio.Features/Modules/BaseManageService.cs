using Apex.Portfolio.Abstracts.Details;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Exceptions;
using Apex.Shared.Core.Pagination.Abstract;
using Apex.Shared.Core.Pagination.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Apex.Portfolio.Modules;

public abstract class BaseManageService<TModel, TEntity, TFilter, TQuery>(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IBaseDetailsMapper<TModel, TEntity, TFilter, TQuery> mapper
) : IBaseDetailsService<TModel, TEntity, TFilter>
    where TModel : class
    where TEntity : class
    where TFilter : class
    where TQuery : IBaseQuery<TEntity>
{
    public IBaseDetailsMapper<TModel, TEntity, TFilter, TQuery> Mapper => mapper;

    protected virtual string GetNotFoundMessage(object? context) => $"{typeof(TEntity)} was not found";

    protected virtual async Task<TEntity?> FindEntity(EntitiesDbContext db, TFilter filter, bool asNoTracking, CancellationToken cancellationToken)
    {
        var query = db.Set<TEntity>().ByQuery(mapper.MapToQuery(filter));

        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TModel> Create(TModel model, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        await db.BeginTransactionAsync();

        var entity = mapper.MapToEntity(model);

        await db.Set<TEntity>().AddAsync(entity, cancellationToken);

        await db.CommitTransactionAsync();

        return await Get(mapper.MapToFilter(entity), cancellationToken);
    }

    public virtual async Task<TModel> Update(TFilter filter, TModel model, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        await db.BeginTransactionAsync();

        var entity = await FindEntity(db, filter, asNoTracking: false, cancellationToken)
            ?? throw new NotFoundException(GetNotFoundMessage(filter));

        mapper.MapData(model, entity);

        await db.CommitTransactionAsync();

        return await Get(mapper.MapToFilter(entity), cancellationToken);
    }

    public virtual async Task Delete(TFilter filter, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        await db.BeginTransactionAsync();

        var entity = await FindEntity(db, filter, asNoTracking: false, cancellationToken)
            ?? throw new NotFoundException(GetNotFoundMessage(filter));

        db.Set<TEntity>().Remove(entity);

        await db.CommitTransactionAsync();
    }

    public async Task<TModel> Get(TFilter filter, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var entity = await FindEntity(db, filter, asNoTracking: true, cancellationToken)
            ?? throw new NotFoundException(GetNotFoundMessage(filter));

        return mapper.MapToModel(entity);
    }
}
