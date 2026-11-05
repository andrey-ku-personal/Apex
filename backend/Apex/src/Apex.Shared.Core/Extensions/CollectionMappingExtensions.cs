namespace Apex.Shared.Core.Extensions;

public static class CollectionMappingExtensions
{
    public static void MergeCollections<TModel, TEntity, TKey>(
        this ICollection<TEntity> destination,
        IReadOnlyCollection<TModel>? source,
        Func<TEntity, TKey> destKeyFunc,
        Func<TModel, TKey> sourceKeyFunc,
        Func<TModel, TEntity> createAction,
        Action<TModel, TEntity> updateAction)
        where TEntity : class
        where TKey : notnull
    {
        destination ??= [];
        source ??= [];
        
        var defaultKey = default(TKey);

        var destinationHash = destination.ToDictionary(destKeyFunc);

        foreach (var modelItem in source)
        {
            var sourceKey = sourceKeyFunc(modelItem);

            if (sourceKey!.Equals(defaultKey))
                destination.Add(createAction(modelItem));
            else if (destinationHash.Remove(sourceKey, out var existing))
                updateAction(modelItem, existing);
        }

        foreach (var entityItem in destinationHash.Values.ToList())
            destination.Remove(entityItem);
    }
}
