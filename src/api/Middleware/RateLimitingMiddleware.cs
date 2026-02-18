using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace TuviApi.Middleware;

public class RateLimitingMiddleware : IFunctionsWorkerMiddleware
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<RateLimitingMiddleware> _logger;
    private const int Limit = 60;
    private static readonly System.TimeSpan Period = System.TimeSpan.FromMinutes(1);

    public RateLimitingMiddleware(IMemoryCache cache, ILogger<RateLimitingMiddleware> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var httpReq = await context.GetHttpRequestDataAsync();
        var ipAddress = httpReq?.GetClientIpAddress();

        if (string.IsNullOrEmpty(ipAddress))
        {
            await next(context);
            return;
        }

        var cacheKey = $"RateLimit_{ipAddress}";
        var entry = _cache.Get<RateLimitEntry>(cacheKey);

        if (entry != null && entry.Count >= Limit && System.DateTime.UtcNow < entry.ExpiresAt)
        {
            _logger.LogWarning("Rate limit exceeded for IP address: {IPAddress}", ipAddress);
            var httpResponse = context.GetHttpResponseData();
            if (httpResponse != null)
            {
                httpResponse.StatusCode = HttpStatusCode.TooManyRequests;
                await httpResponse.WriteStringAsync("Rate limit exceeded.");
                context.GetInvocationResult().Value = httpResponse;
            }
            return;
        }

        var newCount = (entry?.Count ?? 0) + 1;
        var newEntry = new RateLimitEntry
        {
            Count = newCount,
            ExpiresAt = entry?.ExpiresAt ?? System.DateTime.UtcNow.Add(Period)
        };

        _cache.Set(cacheKey, newEntry, newEntry.ExpiresAt);

        await next(context);
    }
}

public static class HttpRequestDataExtensions
{
    public static string? GetClientIpAddress(this HttpRequestData req)
    {
        if (req.Headers.TryGetValues("X-Forwarded-For", out var values))
        {
            return values.FirstOrDefault()?.Split(',').First().Trim();
        }
        return req.Headers.TryGetValues("REMOTE_ADDR", out values) ? values.FirstOrDefault() : null;
    }
}


public class RateLimitEntry
{
    public int Count { get; set; }
    public System.DateTime ExpiresAt { get; set; }
}
