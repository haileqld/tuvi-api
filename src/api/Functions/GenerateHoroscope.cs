using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using TuviApi.Models;
using TuviApi.Services;
using System.Text.Json;
using System.IO;

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
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(TuviApi.Models.ProblemDetails), Summary = "Invalid Request", Description = "The request body is invalid or missing required fields.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(TuviApi.Models.ProblemDetails), Summary = "Internal Error", Description = "An unexpected error occurred.")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/horoscope/generate")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        try
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonSerializer.Deserialize<HoroscopeGenerateRequest>(requestBody, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            // Basic validation
            if (request == null || request.GregorianBirthDate == default)
            {
                var problem = new TuviApi.Models.ProblemDetails
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "Invalid Request",
                    Detail = "Request body is missing or invalid."
                };
                return new BadRequestObjectResult(problem);
            }

            var result = await _horoscopeService.GenerateAsync(request);
            return new OkObjectResult(new SuccessEnvelope<HoroscopeGenerateResponse> { Data = result });
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize request body.");
            var problem = new TuviApi.Models.ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest,
                Title = "Invalid Request",
                Detail = "The request body is not a valid JSON or does not match the expected format."
            };
            return new BadRequestObjectResult(problem);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "An error occurred while generating the horoscope.");
            var problem = new TuviApi.Models.ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "An internal error occurred.",
                Detail = "An unexpected error prevented the request from completing."
            };
            return new ObjectResult(problem) { StatusCode = (int)HttpStatusCode.InternalServerError };
        }
    }
}
