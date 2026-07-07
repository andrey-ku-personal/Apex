using System.Collections.Concurrent;

namespace Apex.Invest.Factories;

public class MapperFactory : IMapperFactory
{
    private readonly ConcurrentDictionary<Type, Func<object>> _mappers = new();

    public void RegisterMapper<TMapper>(Func<TMapper> factory)
        where TMapper : class
        => _mappers.AddOrUpdate(typeof(TMapper), factory, (key, old) => factory);

    public TMapper GetMapper<TMapper>()
        where TMapper : class
    {
        if (!_mappers.TryGetValue(typeof(TMapper), out var factory))
            throw new InvalidOperationException($"Mapper {typeof(TMapper).Name} is not registered.");

        return (TMapper)factory();
    }
}
