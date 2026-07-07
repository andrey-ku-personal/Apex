namespace Apex.Shared.Core.Extensions;

public static class StringExtension
{
    public static bool IsEmpty(this string? data)
        => string.IsNullOrWhiteSpace(data);
}
