namespace Apex.Shared.Core.Extensions;

public static class EnumerableExtension
{
    public static bool HasAny<T>(this IEnumerable<T>? enumeration)
        => enumeration != null && enumeration.Any();

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(action);

        foreach (var item in source)
        {
            action(item);
        }
    }
}