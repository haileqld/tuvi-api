using Azure.AI.OpenAI;
using OpenAI.Chat;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text.Json;
using System.Threading.Tasks;
using TuviApi.Models;
using System.ClientModel;

namespace TuviApi.Services;

public interface IAiInterpretationService
{
    Task<IEnumerable<InterpretationItem>> GetInterpretationAsync(TechnicalChart chart, string language);
}

public class AiInterpretationService : IAiInterpretationService
{
    private readonly AzureOpenAIClient _openAIClient;
    private readonly ILogger<AiInterpretationService> _logger;
    private readonly ResourceManager _resourceManager;
    private readonly string _deploymentName;

    public AiInterpretationService(AzureOpenAIClient openAIClient, ILogger<AiInterpretationService> logger, IConfiguration configuration)
    {
        _openAIClient = openAIClient;
        _logger = logger;
        _resourceManager = new ResourceManager("TuviApi.Prompts", typeof(AiInterpretationService).Assembly);
        _deploymentName = configuration["AZURE_OPENAI_DEPLOYMENT_NAME"] ?? "gpt-4";
    }

    public async Task<IEnumerable<InterpretationItem>> GetInterpretationAsync(TechnicalChart chart, string language)
    {
        var culture = new CultureInfo(language);
        var systemPrompt = _resourceManager.GetString("SystemPrompt", culture) 
            ?? _resourceManager.GetString("SystemPrompt", CultureInfo.InvariantCulture);

        var userPrompt = JsonSerializer.Serialize(chart);

        var chatClient = _openAIClient.GetChatClient(_deploymentName);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(userPrompt)
        };

        var options = new ChatCompletionOptions
        {
            ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat(),
            Temperature = 0.5f,
        };

        try
        {
            // Set a 30-second timeout for the AI call as per FR-018
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            
            ClientResult<ChatCompletion> response = await chatClient.CompleteChatAsync(messages, options, cancellationTokenSource.Token);

            if (response.Value.Usage != null)
            {
                _logger.LogInformation("Azure OpenAI token usage for interpretation: Prompt={PromptTokens}, Completion={CompletionTokens}, Total={TotalTokens}",
                    response.Value.Usage.InputTokenCount,
                    response.Value.Usage.OutputTokenCount,
                    response.Value.Usage.TotalTokenCount);
            }

            var interpretationJson = response.Value.Content[0].Text;
            _logger.LogDebug("AI Response JSON: {Json}", interpretationJson);

            // Attempt to deserialize directly as a list
            try 
            {
                var interpretation = JsonSerializer.Deserialize<List<InterpretationItem>>(interpretationJson);
                return interpretation ?? new List<InterpretationItem>();
            }
            catch (JsonException)
            {
                // If direct list deserialization fails, try to see if it's wrapped in an object (common in JSON mode)
                var doc = JsonDocument.Parse(interpretationJson);
                if (doc.RootElement.ValueKind == JsonValueKind.Object)
                {
                    // Look for common wrapper property names
                    foreach (var property in doc.RootElement.EnumerateObject())
                    {
                        if (property.Value.ValueKind == JsonValueKind.Array)
                        {
                            var interpretation = property.Value.Deserialize<List<InterpretationItem>>();
                            return interpretation ?? new List<InterpretationItem>();
                        }
                    }
                }
                throw;
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogError("AI interpretation request timed out after 30 seconds.");
            throw new Exception("The AI interpretation service timed out.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get or parse AI interpretation.");
            return new List<InterpretationItem> { 
                new InterpretationItem { 
                    AreaName = "Error", 
                    Headline = "Interpretation Unavailable", 
                    Detail = "We're sorry, but the AI was unable to generate an interpretation at this time." 
                } 
            };
        }
    }
}
