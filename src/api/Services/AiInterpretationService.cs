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
    private readonly ChatClient _chatClient;
    private readonly ILogger<AiInterpretationService> _logger;
    private readonly ResourceManager _resourceManager;

    public AiInterpretationService(ChatClient chatClient, ILogger<AiInterpretationService> logger)
    {
        _chatClient = chatClient;
        _logger = logger;
        _resourceManager = new ResourceManager("TuviApi.Prompts", typeof(AiInterpretationService).Assembly);
    }

    public async Task<IEnumerable<InterpretationItem>> GetInterpretationAsync(TechnicalChart chart, string language)
    {
        var culture = new CultureInfo(language);
        var systemPrompt = _resourceManager.GetString("SystemPrompt", culture) 
            ?? _resourceManager.GetString("SystemPrompt", CultureInfo.InvariantCulture);

        var userPrompt = JsonSerializer.Serialize(chart);

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
            // Set a 120-second timeout for the AI call to accommodate local LLMs
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1200));
            
            ClientResult<ChatCompletion> response = await _chatClient.CompleteChatAsync(messages, options, cancellationTokenSource.Token);

            if (response.Value.Usage != null)
            {
                _logger.LogInformation("Azure OpenAI token usage for interpretation: Prompt={PromptTokens}, Completion={CompletionTokens}, Total={TotalTokens}",
                    response.Value.Usage.InputTokenCount,
                    response.Value.Usage.OutputTokenCount,
                    response.Value.Usage.TotalTokenCount);
            }

            if (response.Value.Content == null || response.Value.Content.Count == 0)
            {
                _logger.LogWarning("AI response content is empty.");
                return new List<InterpretationItem> { 
                    new InterpretationItem { 
                        AreaName = "Error", 
                        Headline = "Interpretation Unavailable", 
                        Detail = "The AI returned an empty response." 
                    } 
                };
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
            _logger.LogError("AI interpretation request timed out after 120 seconds.");
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
