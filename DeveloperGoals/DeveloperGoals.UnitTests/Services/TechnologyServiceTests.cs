using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using DeveloperGoals.Services;
using DeveloperGoals.DTOs;

namespace DeveloperGoals.UnitTests.Services;

/// <summary>
/// Testy jednostkowe dla TechnologyService
/// </summary>
public class TechnologyServiceTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly Mock<ILogger<TechnologyService>> _loggerMock;
    private readonly Mock<IUserStateService> _userStateServiceMock;
    private readonly TechnologyService _sut; // System Under Test

    public TechnologyServiceTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://localhost:5001")
        };
        _loggerMock = new Mock<ILogger<TechnologyService>>();
        _userStateServiceMock = new Mock<IUserStateService>();
        
        // Domyślny stan użytkownika
        _userStateServiceMock.Setup(x => x.CurrentState)
            .Returns(new UserState { Email = "test@example.com" });

        _sut = new TechnologyService(_httpClient, _loggerMock.Object, _userStateServiceMock.Object);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }

    #region GetGraphAsync Tests

    [Fact]
    public async Task GetGraphAsync_WhenSuccessful_ShouldReturnGraph()
    {
        // Arrange
        var expectedGraph = new TechnologyGraphDto
        {
            Nodes = new List<GraphNodeDto>
            {
                new() { Id = 1, Name = "C#" },
                new() { Id = 2, Name = ".NET" }
            },
            Edges = new List<GraphEdgeDto>
            {
                new() { Id = 1, From = 1, To = 2 }
            },
            Stats = new GraphStatsDto()
        };

        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(expectedGraph)
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _sut.GetGraphAsync();

        // Assert
        result.Should().NotBeNull();
        result!.Nodes.Should().HaveCount(2);
        result.Edges.Should().HaveCount(1);
        result.Nodes.First().Name.Should().Be("C#");
    }

    [Fact]
    public async Task GetGraphAsync_WhenHttpError_ShouldReturnNull()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _sut.GetGraphAsync();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetGraphAsync_WhenUserNotLoggedIn_ShouldCallGraphEndpointWithoutEmail()
    {
        // Arrange
        _userStateServiceMock.Setup(x => x.CurrentState)
            .Returns(new UserState { Email = string.Empty });

        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new TechnologyGraphDto 
            { 
                Nodes = new List<GraphNodeDto>(), 
                Edges = new List<GraphEdgeDto>(),
                Stats = new GraphStatsDto()
            })
        };

        HttpRequestMessage? capturedRequest = null;
        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .Callback<HttpRequestMessage, CancellationToken>((req, _) => capturedRequest = req)
            .ReturnsAsync(httpResponse);

        // Act
        await _sut.GetGraphAsync();

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.RequestUri!.ToString().Should().EndWith("/api/technologies/graph");
        capturedRequest.RequestUri.ToString().Should().NotContain("email=");
    }

    #endregion

    #region AddTechnologyAsync Tests

    [Fact]
    public async Task AddTechnologyAsync_WhenCommandIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        Func<Task> act = async () => await _sut.AddTechnologyAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task AddTechnologyAsync_WhenSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var command = new CreateTechnologyCommand
        {
            TechnologyDefinitionId = 1,
            FromTechnologyId = 2,
            IsCustom = false
        };

        var expectedResponse = new CreateTechnologyResponseDto
        {
            Technology = new TechnologyDto { Id = 1, Name = "C#" },
            Dependency = new DependencyDto { Id = 1, FromTechnologyId = 1, ToTechnologyId = 2 }
        };

        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(expectedResponse)
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _sut.AddTechnologyAsync(command);

        // Assert
        result.Should().NotBeNull();
        result!.Technology.Should().NotBeNull();
        result.Technology.Id.Should().Be(1);
    }

    #endregion

    #region DeleteTechnologyAsync Tests

    [Fact]
    public async Task DeleteTechnologyAsync_WhenIdIsZeroOrNegative_ShouldThrowArgumentException()
    {
        // Act
        Func<Task> act1 = async () => await _sut.DeleteTechnologyAsync(0);
        Func<Task> act2 = async () => await _sut.DeleteTechnologyAsync(-1);

        // Assert
        await act1.Should().ThrowAsync<ArgumentException>();
        await act2.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task DeleteTechnologyAsync_WhenSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var technologyId = 1;
        var expectedResponse = new DeleteTechnologyResponseDto
        {
            Message = "Technologia została usunięta",
            DeletedDependencies = 2
        };

        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(expectedResponse)
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _sut.DeleteTechnologyAsync(technologyId);

        // Assert
        result.Should().NotBeNull();
        result!.Message.Should().Be("Technologia została usunięta");
        result.DeletedDependencies.Should().Be(2);
    }

    #endregion
}
