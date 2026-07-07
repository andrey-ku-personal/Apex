using Newtonsoft.Json;

namespace Apex.Shared.Core.Helpers;

public static class HashHelper
{
    public static int GetHash(object origin) => JsonConvert.SerializeObject(origin).GetHashCode();
}