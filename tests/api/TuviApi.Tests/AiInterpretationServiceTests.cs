using Moq;
using TuviApi.Models;
using TuviApi.Services;
using Xunit;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.ClientModel;
using System.Reflection;

namespace TuviApi.Tests;

public class AiInterpretationServiceTests
{
    private readonly Mock<AzureOpenAIClient> _openAIClientMock;
    private readonly Mock<ILogger<AiInterpretationService>> _loggerMock;
    private readonly Mock<ChatClient> _chatClientMock;

    public AiInterpretationServiceTests()
    {
        _openAIClientMock = new Mock<AzureOpenAIClient>();
        _loggerMock = new Mock<ILogger<AiInterpretationService>>();
        _chatClientMock = new Mock<ChatClient>();
    }

    // Since AzureOpenAIClient and ChatClient are hard to mock (they don't have interfaces or many virtual methods in some versions),
    // this test might be difficult without a wrapper.
    // However, in version 2.1.0, let's see.
    
    // I will try to see if I can mock ChatClient.CompleteChatAsync.
    // If not, I should probably wrap the AI client.
}
