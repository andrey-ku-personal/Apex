using Microsoft.EntityFrameworkCore;

namespace Apex.Invest.Persistance.Upsert;

public class UpsertBuilder<TEntity, TSource>(DbSet<TEntity> set, TSource source)
    where TEntity : class, new()
    where TSource : class
{
    private TEntity? _entity;
    private Func<TSource, bool>? _createWhenCondition;

    public UpsertBuilder<TEntity, TSource> OnKey(object? keyValue)
    {
        _entity = set.Find(keyValue);

        return this;
    }

    public UpsertBuilder<TEntity, TSource> CreateWhen(Func<TSource, bool> condition)
    {
        _createWhenCondition = condition;
        return this;
    }

    public UpsertBuilder<TEntity, TSource> OnKey(params object?[] keyValues)
    {
        _entity = set.Find(keyValues);
        return this;
    }

    public UpsertBuilder<TEntity, TSource> Map(Action<TSource, TEntity> mapFn)
    {
        if (_entity == null)
        {
            _entity = new();
            set.Add(_entity);
        }

        mapFn(source, _entity);
        return this;
    }

    public async Task<TEntity> ExecuteAsync(Action<TSource, TEntity> updateFn, CancellationToken cancellationToken)
    {
        if (_entity == null)
            await AddAsync(cancellationToken);

        updateFn(source, _entity!);

        return _entity!;
    }

    private async Task AddAsync(CancellationToken cancellationToken)
    {
        if (_createWhenCondition != null && !_createWhenCondition(source))
            throw new InvalidOperationException($"Cannot create {typeof(TEntity).Name} - create condition was not met.");

        _entity = new();
        await set.AddAsync(_entity, cancellationToken);
    }
}
