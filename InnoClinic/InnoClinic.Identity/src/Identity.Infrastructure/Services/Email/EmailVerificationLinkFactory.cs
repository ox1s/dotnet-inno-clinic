using Identity.Application.Common.Interfaces;
using Identity.Infrastructure.Exceptions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;

namespace Identity.Infrastructure.Services.Email;

public class EmailVerificationLinkFactory(
    IHttpContextAccessor httpContextAccessor,
    LinkGenerator linkGenerator,
    IConfiguration configuration) : IEmailVerificationLinkFactory
{
    public string Create(Guid accountId, string token)
    {
        var httpContext = httpContextAccessor.HttpContext;

        string? uri;

        if (httpContext is not null)
        {
            uri = linkGenerator.GetUriByName(
                httpContext,
                "VerifyEmailRoute",
                new { accountId, token });
        }
        else
        {
            var appUrl = configuration["AppUrl"];
            if (string.IsNullOrEmpty(appUrl))
            {
                throw new EmailSendingException("AppUrl is not configured. Cannot generate email link in background.");
            }

            if (!Uri.TryCreate(appUrl, UriKind.Absolute, out var baseUrl))
            {
                throw new EmailSendingException($"Invalid AppUrl configuration: {appUrl}");
            }


            uri = linkGenerator.GetUriByName(
                "VerifyEmailRoute",
                new { accountId, token},
                scheme: baseUrl.Scheme,
                host: HostString.FromUriComponent(baseUrl));
        }

        return uri ?? throw new Exception("Could not generate email verification link");
    }
}
