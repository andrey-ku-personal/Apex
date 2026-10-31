using Apex.Portfolio.Abstracts.Details;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Exceptions;
using Apex.Shared.Core.Pagination.Abstract;
using Apex.Shared.Core.Pagination.Expressions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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
    protected IBaseDetailsMapper<TModel, TEntity, TFilter, TQuery> Mapper => mapper;

    protected virtual string GetNotFoundMessage(object? context) => $"{typeof(TEntity)} was not found";

    protected virtual Task ValidateDeleteAsync(EntitiesDbContext db, TEntity entity, CancellationToken cancellationToken) =>
        Task.CompletedTask;

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

        await db.BeginTransactionAsync(cancellationToken);

        var entity = mapper.MapToEntity(model);

        await db.Set<TEntity>().AddAsync(entity, cancellationToken);

        try
        {
            await db.CommitTransactionAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new ConflictException($"{typeof(TEntity).Name} already exists");
        }

        return await Get(mapper.MapToFilter(entity), cancellationToken);
    }

    public virtual async Task<TModel> Update(TFilter filter, TModel model, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        await db.BeginTransactionAsync(cancellationToken);

        var entity = await FindEntity(db, filter, asNoTracking: false, cancellationToken)
            ?? throw new NotFoundException(GetNotFoundMessage(filter));

        mapper.MapData(model, entity);

        await db.CommitTransactionAsync(cancellationToken);

        return await Get(mapper.MapToFilter(entity), cancellationToken);
    }

    public virtual async Task Delete(TFilter filter, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        await db.BeginTransactionAsync(cancellationToken);

        var entity = await FindEntity(db, filter, asNoTracking: false, cancellationToken)
            ?? throw new NotFoundException(GetNotFoundMessage(filter));

        await ValidateDeleteAsync(db, entity, cancellationToken);

        db.Set<TEntity>().Remove(entity);

        await db.CommitTransactionAsync(cancellationToken);
    }

    public async Task<TModel> Get(TFilter filter, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var entity = await FindEntity(db, filter, asNoTracking: true, cancellationToken)
            ?? throw new NotFoundException(GetNotFoundMessage(filter));

        return mapper.MapToModel(entity);
    }
}
