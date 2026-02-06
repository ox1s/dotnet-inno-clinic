using Identity.Domain.AccountAggregate;

namespace Identity.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(Account account, string role);
}