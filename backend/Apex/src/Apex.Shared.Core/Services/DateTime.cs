namespace Apex.Shared.Core.Services;
public class DateTime : IDateTime
{
    public System.DateTime GetUtcDate() => System.DateTime.UtcNow;
}
