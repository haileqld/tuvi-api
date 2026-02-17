using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using TuviApi.Models;
using TuviApi.Services;

namespace TuviApi.Functions;

public class GenerateHoroscope
{
    private readonly ILogger<GenerateHoroscope> _logger;
    private readonly IHoroscopeService _horoscopeService;

    public GenerateHoroscope(ILogger<GenerateHoroscope> logger, IHoroscopeService horoscopeService)
    {
        _logger = logger;
        _horoscopeService = horoscopeService;
    }

    [Function("GenerateHoroscope")]
    [OpenApiOperation(operationId: "generateHoroscope", tags: new[] { "Horoscope" }, Summary = "Generate a Horoscope Interpretation", Description = "This endpoint generates a Vietnamese Tử Vi horoscope based on the user's birth details.", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(HoroscopeGenerateRequest), Required = true, Description = "User birth details and request options.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SuccessEnvelope<HoroscopeGenerateResponse>), Summary = "Successful generation", Description = "The horoscope interpretation was successfully generated.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ProblemDetails), Summary = "Invalid Request", Description = "The request body is invalid or missing required fields.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(ProblemDetails), Summary = "Internal Error", Description = "An unexpected error occurred.")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/horoscope/generate")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var request = await req.ReadFromJsonAsync<HoroscopeGenerateRequest>();

        // Basic validation
        if (request == null || request.GregorianBirthDate == default)
        {
            var problem = new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest,
                Title = "Invalid Request",
                Detail = "Request body is missing or invalid."
            };
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await errorResponse.WriteAsJsonAsync(problem);
            return errorResponse;
        }

        try
        {
            var result = await _horoscopeService.GenerateAsync(request);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new SuccessEnvelope<HoroscopeGenerateResponse> { Data = result });
            return response;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "An error occurred while generating the horoscope.");
            var problem = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "An internal error occurred.",
                Detail = "An unexpected error prevented the request from completing."
            };
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteAsJsonAsync(problem);
            return errorResponse;
        }
    }
}
