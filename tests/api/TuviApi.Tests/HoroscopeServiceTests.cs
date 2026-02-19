using Moq;
using TuviApi.Lib.Charting;
using TuviApi.Models;
using TuviApi.Services;
using Xunit;

namespace TuviApi.Tests;

public class HoroscopeServiceTests
{
    private readonly Mock<IChartingService> _chartingServiceMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IAiInterpretationService> _aiInterpretationServiceMock;
    private readonly HoroscopeService _service;

    public HoroscopeServiceTests()
    {
        _chartingServiceMock = new Mock<IChartingService>();
        _cacheServiceMock = new Mock<ICacheService>();
        _aiInterpretationServiceMock = new Mock<IAiInterpretationService>();
        _service = new HoroscopeService(
            _chartingServiceMock.Object,
            _cacheServiceMock.Object,
            _aiInterpretationServiceMock.Object);
    }

    [Fact]
    public async Task GenerateAsync_ReturnsCachedResponse_WhenAvailable()
    {
        // Arrange
        var request = new HoroscopeGenerateRequest { GregorianBirthDate = DateTime.Now, Language = "en" };
        var cachedResponse = new HoroscopeGenerateResponse { Interpretation = new List<InterpretationItem>() };
        _cacheServiceMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(cachedResponse);

        // Act
        var result = await _service.GenerateAsync(request);

        // Assert
        Assert.Same(cachedResponse, result);
        _chartingServiceMock.Verify(x => x.GenerateChart(It.IsAny<HoroscopeGenerateRequest>()), Times.Never);
    }

    [Fact]
    public async Task GenerateAsync_CallsServices_WhenCacheMiss()
    {
        // Arrange
        var request = new HoroscopeGenerateRequest { GregorianBirthDate = DateTime.Now, Language = "en" };
        _cacheServiceMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync((HoroscopeGenerateResponse?)null);
        
        var technicalChart = new TechnicalChart();
        _chartingServiceMock.Setup(x => x.GenerateChart(request)).Returns(technicalChart);
        
        var interpretation = new List<InterpretationItem>();
        _aiInterpretationServiceMock.Setup(x => x.GetInterpretationAsync(technicalChart, request.Language)).ReturnsAsync(interpretation);

        // Act
        var result = await _service.GenerateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(interpretation, result.Interpretation);
        _cacheServiceMock.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<HoroscopeGenerateResponse>()), Times.Once);
    }
}
