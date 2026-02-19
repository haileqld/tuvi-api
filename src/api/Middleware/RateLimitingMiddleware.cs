using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http; // For HttpContext
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
        // For ASP.NET Core integration, we use HttpContext
        var httpContext = context.GetHttpContext();

        if (httpContext == null)
        {
            // Not an HTTP trigger or something went wrong
            await next(context);
            return;
        }

        var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();

        if (string.IsNullOrEmpty(ipAddress))
        {
            // Fallback to X-Forwarded-For if behind a proxy
            if (httpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            {
                ipAddress = forwardedFor.FirstOrDefault()?.Split(',').First().Trim();
            }
        }

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
            
            httpContext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            await httpContext.Response.WriteAsync("Rate limit exceeded.");
            
            // Short-circuit the pipeline
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

public class RateLimitEntry
{
    public int Count { get; set; }
    public System.DateTime ExpiresAt { get; set; }
}
