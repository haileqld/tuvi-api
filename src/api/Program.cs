using Azure.AI.OpenAI;
using Azure.Core;
using Azure.Data.Tables;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using TuviApi.Lib.Charting;
using TuviApi.Middleware;
using TuviApi.Services;

var builder = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(worker =>
    {
        worker.UseMiddleware<RateLimitingMiddleware>();
    })
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        var allowedOrigins = Environment.GetEnvironmentVariable("AllowedOrigins", EnvironmentVariableTarget.Process)?.Split(";") ?? new[] { "http://localhost:3000" };
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins",
                builder => builder.WithOrigins(allowedOrigins)
                                  .AllowAnyHeader()
                                  .AllowAnyMethod());
        });

        // Add Memory Cache for rate limiting
        services.AddMemoryCache();

        // Register application services
        services.AddSingleton<IHoroscopeService, HoroscopeService>();
        services.AddSingleton<IAiInterpretationService, AiInterpretationService>();
        services.AddSingleton<ICacheService, CacheService>();
        services.AddSingleton<IChartingService, ChartingService>();

        // Configure Azure Clients
        services.AddSingleton(provider =>
        {
            var endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT", EnvironmentVariableTarget.Process);
            ArgumentException.ThrowIfNullOrEmpty(endpoint);
            return new OpenAIClient(new Uri(endpoint), new DefaultAzureCredential());
        });

        services.AddSingleton(provider =>
        {
            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage", EnvironmentVariableTarget.Process);
            ArgumentException.ThrowIfNullOrEmpty(connectionString);
            return new TableServiceClient(connectionString);
        });
    })
    .Build();

builder.Run();
