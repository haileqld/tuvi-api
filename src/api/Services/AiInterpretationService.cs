using Azure.AI.OpenAI;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text.Json;
using System.Threading.Tasks;
using TuviApi.Models;

namespace TuviApi.Services;

public interface IAiInterpretationService
{
    Task<IEnumerable<InterpretationItem>> GetInterpretationAsync(TechnicalChart chart, string language);
}

public class AiInterpretationService : IAiInterpretationService
{
    private readonly OpenAIClient _openAIClient;
    private readonly ILogger<AiInterpretationService> _logger;
    private readonly ResourceManager _resourceManager;
    private const string DeploymentName = "gpt-4"; // Example deployment name

    public AiInterpretationService(OpenAIClient openAIClient, ILogger<AiInterpretationService> logger)
    {
        _openAIClient = openAIClient;
        _logger = logger;
        _resourceManager = new ResourceManager("TuviApi.Prompts", typeof(AiInterpretationService).Assembly);
    }

    public async Task<IEnumerable<InterpretationItem>> GetInterpretationAsync(TechnicalChart chart, string language)
    {
        var culture = new CultureInfo(language);
        var systemPrompt = _resourceManager.GetString("SystemPrompt", culture) 
            ?? _resourceManager.GetString("SystemPrompt", CultureInfo.InvariantCulture);

        var userPrompt = JsonSerializer.Serialize(chart);

        var chatCompletionsOptions = new ChatCompletionsOptions()
        {
            DeploymentName = DeploymentName,
            Messages =
            {
                new ChatRequestSystemMessage(systemPrompt),
                new ChatRequestUserMessage(userPrompt),
            },
            ResponseFormat = ChatCompletionsResponseFormat.JsonObject,
            Temperature = 0.5f,
        };

        // Set a 30-second timeout for the AI call as per FR-018
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var response = await _openAIClient.GetChatCompletionsAsync(chatCompletionsOptions, cancellationTokenSource.Token);

        _logger.LogInformation("Azure OpenAI token usage for interpretation: Prompt={PromptTokens}, Completion={CompletionTokens}, Total={TotalTokens}",
            response.Value.Usage.PromptTokens,
            response.Value.Usage.CompletionTokens,
            response.Value.Usage.TotalTokens);

        var interpretationJson = response.Value.Choices[0].Message.Content;
        var interpretation = JsonSerializer.Deserialize<List<InterpretationItem>>(interpretationJson);

        return interpretation ?? new List<InterpretationItem>();
    }
}
