using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Profile.Infrastructure.Auth;

public class BotApiKeyHandler(
    IConfiguration configuration,
    IHttpContextAccessor httpContextAccessor)
    : AuthorizationHandler<BotApiKeyRequirement>
{
    private const string ApiKeyHeaderName = "X-Api-Key";

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        BotApiKeyRequirement requirement)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return Task.CompletedTask;
        }

        if (!httpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Fail();
            return Task.CompletedTask;
        }
        var expectedApiKey = configuration.GetValue<string>("BotSettings:ApiKey");

        if (expectedApiKey is not null && extractedApiKey == expectedApiKey)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}