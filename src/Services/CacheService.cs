using Azure.Data.Tables;
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
    private readonly TableClient _tableClient;

    public CacheService(TableServiceClient tableServiceClient)
    {
        tableServiceClient.CreateTableIfNotExists(TableName);
        _tableClient = tableServiceClient.GetTableClient(TableName);
    }

    public async Task<HoroscopeGenerateResponse?> GetAsync(string key)
    {
        var entity = await _tableClient.GetEntityIfExistsAsync<CacheEntity>(key, key);
        if (entity.HasValue)
        {
            return JsonSerializer.Deserialize<HoroscopeGenerateResponse>(entity.Value.Data);
        }
        return null;
    }

    public async Task SetAsync(string key, HoroscopeGenerateResponse response)
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
}

public class CacheEntity : ITableEntity
{
    public string PartitionKey { get; set; } = string.Empty;
    public string RowKey { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public System.DateTimeOffset? Timestamp { get; set; }
    public Azure.ETag ETag { get; set; }
}
