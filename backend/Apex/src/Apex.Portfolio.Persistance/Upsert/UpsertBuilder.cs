using Microsoft.EntityFrameworkCore;

namespace Apex.Portfolio.Persistance.Upsert;

public class UpsertBuilder<TEntity, TSource>(DbSet<TEntity> set, TSource source)
    where TEntity : class
    where TSource : class
{
    private TEntity? _entity;
    private Func<TSource, bool>? _createWhenCondition;
    private Func<TSource, TEntity>? _createWithFactory;

    public UpsertBuilder<TEntity, TSource> OnKey(Func<TSource, object[]> keySelector)
    {
        var keys = keySelector(source);
        _entity = set.Find(keys);
        return this;
    }

    public UpsertBuilder<TEntity, TSource> CreateWhen(Func<TSource, bool> condition)
    {
        _createWhenCondition = condition;
        return this;
    }

    public UpsertBuilder<TEntity, TSource> CreateWith(Func<TSource, TEntity> factory)
    {
        _createWithFactory = factory;
        return this;
    }

    public async Task<TEntity> ExecuteAsync(Action<TSource, TEntity> updateFn, CancellationToken cancellationToken)
    {
        var isNew = _entity == null;

        if (isNew)
        {
            if (_createWhenCondition != null && !_createWhenCondition(source))
                throw new InvalidOperationException($"Cannot create {typeof(TEntity).Name} - create condition was not met.");

            _entity = _createWithFactory is not null
                ? _createWithFactory(source)
                : throw new InvalidOperationException($"Cannot create {typeof(TEntity).Name} - CreateWith factory was not configured.");
            await set.AddAsync(_entity, cancellationToken);
        }

        updateFn(source, _entity!);

        return _entity!;
    }
}
