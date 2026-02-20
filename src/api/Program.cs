using OpenAI.Chat;
using Azure.AI.OpenAI;
using Azure.Core;
using Azure.Identity;
using Azure.Data.Tables;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using TuviApi.Lib.Charting;
using TuviApi.Middleware;
using TuviApi.Services;

var builder = new HostBuilder()
    .ConfigureFunctionsWebApplication(worker =>
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

        // Configure AI Chat Client
        services.AddSingleton<ChatClient>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var endpoint = configuration["AI_ENDPOINT"];
            var model = configuration["AI_MODEL"] ?? "gpt-4";
            var apiKey = configuration["AI_API_KEY"] ?? "placeholder";

            if (!string.IsNullOrEmpty(endpoint) && endpoint.Contains("openai.azure.com"))
            {
                // Azure OpenAI
                var azureClient = new AzureOpenAIClient(new Uri(endpoint), new DefaultAzureCredential());
                return azureClient.GetChatClient(model);
            }
            else if (!string.IsNullOrEmpty(endpoint))
            {
                // Generic OpenAI (Ollama, local LLM, etc.)
                var options = new OpenAI.OpenAIClientOptions();
                options.Endpoint = new Uri(endpoint);
                
                var client = new OpenAI.OpenAIClient(new System.ClientModel.ApiKeyCredential(apiKey), options);
                return client.GetChatClient(model);
            }
            else
            {
                // Fallback / Placeholder
                var azureClient = new AzureOpenAIClient(new Uri("https://placeholder.openai.azure.com"), new DefaultAzureCredential());
                return azureClient.GetChatClient(model);
            }
        });

        services.AddSingleton(provider =>
        {
            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage", EnvironmentVariableTarget.Process);
            if (string.IsNullOrEmpty(connectionString)) return new TableServiceClient("UseDevelopmentStorage=true");

            return new TableServiceClient(connectionString);
        });
    })
    .Build();

builder.Run();
