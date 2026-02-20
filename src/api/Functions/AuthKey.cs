using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using TuviApi.Models;

namespace TuviApi.Functions;

public class AuthKey
{
    [Function("GetAuthKey")]
    [OpenApiOperation(operationId: "getAuthKey", tags: new[] { "Auth" }, Summary = "Retrieve an ephemeral API key", Description = "This endpoint provides an ephemeral API key for client-side use.", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SuccessEnvelope<ApiKeyResponse>), Summary = "Successful retrieval", Description = "The API key was successfully retrieved.")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/auth/key")] HttpRequest req)
    {
        // For local development, we return a placeholder key.
        // In production, this would be more secure.
        var response = new ApiKeyResponse
        {
            ApiKey = "local-dev-key"
        };

        return new OkObjectResult(new SuccessEnvelope<ApiKeyResponse> { Data = response });
    }
}

public class ApiKeyResponse
{
    public string ApiKey { get; set; } = string.Empty;
}
