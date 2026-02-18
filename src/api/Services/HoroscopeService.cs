using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TuviApi.Lib.Charting;
using TuviApi.Models;

namespace TuviApi.Services;

public interface IHoroscopeService
{
    Task<HoroscopeGenerateResponse> GenerateAsync(HoroscopeGenerateRequest request);
}

public class HoroscopeService : IHoroscopeService
{
    private readonly IChartingService _chartingService;
    private readonly ICacheService _cacheService;
    private readonly IAiInterpretationService _aiInterpretationService;

    public HoroscopeService(IChartingService chartingService, ICacheService cacheService, IAiInterpretationService aiInterpretationService)
    {
        _chartingService = chartingService;
        _cacheService = cacheService;
        _aiInterpretationService = aiInterpretationService;
    }

    public async Task<HoroscopeGenerateResponse> GenerateAsync(HoroscopeGenerateRequest request)
    {
        var cacheKey = CreateCacheKey(request);
        var cachedResponse = await _cacheService.GetAsync(cacheKey);
        if (cachedResponse != null)
        {
            return cachedResponse;
        }

        var technicalChart = _chartingService.GenerateChart(request);
        var interpretation = await _aiInterpretationService.GetInterpretationAsync(technicalChart, request.Language);

        var response = new HoroscopeGenerateResponse
        {
            Interpretation = interpretation,
            TechnicalChart = request.IncludeTechnicalDetails ? technicalChart : null
        };

        await _cacheService.SetAsync(cacheKey, response);

        return response;
    }

    private string CreateCacheKey(HoroscopeGenerateRequest request)
    {
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(new
        {
            request.Gender,
            request.GregorianBirthDate,
            request.TimezoneOffset,
            request.Language,
            request.IncludeTechnicalDetails
        }, options);

        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
        return Convert.ToBase64String(bytes);
    }
}
