using System.Reflection;

namespace Apex.Shared.Core.Extensions;

public static class AssemblyExtension
{
    public static Assembly[] GetSolutionAssemblies()
        => [.. AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName?.Contains("Apex") ?? false)];
}
