using System.Threading.RateLimiting;

namespace Apex.Gateway;

public static class InjectionExtension
{
    public static void AddAuthRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddPolicy("auth", context => RateLimitPartition.GetTokenBucketLimiter(
                partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                factory: _ => new TokenBucketRateLimiterOptions
                {
                    TokenLimit = 100,
                    TokensPerPeriod = 1,
                    ReplenishmentPeriod = TimeSpan.FromMilliseconds(600),
                    QueueLimit = 0,
                    AutoReplenishment = true
                }));
        });
    }
}
