using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
using TuviApi.Models;

namespace TuviApi.Services;

public interface ICacheService
{
    Task<HoroscopeGenerateResponse?> GetAsync(string key);
    Task SetAsync(string key, HoroscopeGenerateResponse response);
}

public class CacheService : ICacheService
{
    private const string TableName = "HoroscopeCache";
    private readonly TableClient? _tableClient;
    private readonly ILogger<CacheService> _logger;

    public CacheService(TableServiceClient tableServiceClient, ILogger<CacheService> logger)
    {
        _logger = logger;
        try
        {
            tableServiceClient.CreateTableIfNotExists(TableName);
            _tableClient = tableServiceClient.GetTableClient(TableName);
        }
        catch (System.Exception ex)
        {
            _logger.LogWarning(ex, "Could not initialize Table Storage cache. Caching will be disabled.");
            _tableClient = null;
        }
    }

    public async Task<HoroscopeGenerateResponse?> GetAsync(string key)
    {
        if (_tableClient == null) return null;

        try
        {
            var entity = await _tableClient.GetEntityIfExistsAsync<CacheEntity>(key, key);
            if (entity is { HasValue: true, Value: not null })
            {
                return JsonSerializer.Deserialize<HoroscopeGenerateResponse>(entity.Value.Data);
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogWarning(ex, "Failed to retrieve from cache for key {Key}", key);
        }
        return null;
    }

    public async Task SetAsync(string key, HoroscopeGenerateResponse response)
    {
        if (_tableClient == null) return;

        try
        {
            var data = JsonSerializer.Serialize(response);
            var entity = new CacheEntity
            {
                PartitionKey = key,
                RowKey = key,
                Data = data
            };
            await _tableClient.UpsertEntityAsync(entity);
        }
        catch (System.Exception ex)
        {
            _logger.LogWarning(ex, "Failed to save to cache for key {Key}", key);
        }
    }
}

public class CacheEntity : ITableEntity
{
    public string PartitionKey { get; set; } = string.Empty;
    public string RowKey { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public System.DateTimeOffset? Timestamp { get; set; }
    public Azure.ETag ETag { get; set; }
}
