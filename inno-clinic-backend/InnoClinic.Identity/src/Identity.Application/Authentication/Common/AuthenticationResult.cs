using Identity.Domain.AccountAggregate;

namespace Identity.Application.Authentication.Common;

public record AuthenticationResult(
    Account Account,
    string Token);