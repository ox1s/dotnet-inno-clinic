using System.Security.Claims;

using FluentAssertions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

using Moq;

using Profile.Infrastructure.Auth;

using Xunit;

namespace Profile.Infrastructure.UnitTests.Auth;

public class BotApiKeyHandlerTests
{
    private readonly IConfiguration _configuration;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly BotApiKeyHandler _handler;
    private readonly BotApiKeyRequirement _requirement;

    private const string ValidApiKey = "secret-test-key";

    public BotApiKeyHandlerTests()
    {
        var inMemorySettings = new Dictionary<string, string> {
            {"BotSettings:ApiKey", ValidApiKey}
        };
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        _handler = new BotApiKeyHandler(_configuration, _httpContextAccessorMock.Object);
        _requirement = new BotApiKeyRequirement();
    }

    [Fact]
    public async Task HandleAsync_ShouldSucceed_WhenApiKeyIsValid()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["X-Api-Key"] = ValidApiKey;

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var authContext = new AuthorizationHandlerContext(
            [_requirement],
            new ClaimsPrincipal(),
            null);

        // Act
        await _handler.HandleAsync(authContext);

        // Assert
        authContext.HasSucceeded.Should().BeTrue();
        authContext.HasFailed.Should().BeFalse();
    }

    [Fact]
    public async Task HandleAsync_ShouldFail_WhenApiKeyIsMissing()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var authContext = new AuthorizationHandlerContext(
            [_requirement],
            new ClaimsPrincipal(),
            null);

        // Act
        await _handler.HandleAsync(authContext);

        // Assert
        authContext.HasSucceeded.Should().BeFalse();
        authContext.HasFailed.Should().BeTrue();
    }

    [Fact]
    public async Task HandleAsync_ShouldFail_WhenApiKeyIsInvalid()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["X-Api-Key"] = "wrong-hacker-key";

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var authContext = new AuthorizationHandlerContext(
            [_requirement],
            new ClaimsPrincipal(),
            null);

        // Act
        await _handler.HandleAsync(authContext);

        // Assert
        authContext.HasSucceeded.Should().BeFalse();
        authContext.HasFailed.Should().BeTrue();
    }

    [Fact]
    public async Task HandleAsync_ShouldDoNothing_WhenHttpContextIsNull()
    {
        // Arrange
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext?)null);

        var authContext = new AuthorizationHandlerContext(
            [_requirement],
            new ClaimsPrincipal(),
            null);

        // Act
        await _handler.HandleAsync(authContext);

        // Assert
        authContext.HasSucceeded.Should().BeFalse();
        authContext.HasFailed.Should().BeFalse();
    }
}