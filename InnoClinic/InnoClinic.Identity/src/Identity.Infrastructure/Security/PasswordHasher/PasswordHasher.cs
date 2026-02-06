using System.Text.RegularExpressions;

using ErrorOr;

using Identity.Domain.Common;

namespace Identity.Infrastructure.Security.PasswordHasher;

public partial class PasswordHasher : IPasswordHasher
{
    public ErrorOr<string> HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    public bool IsCorrectPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
}