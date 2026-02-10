using System.Text.RegularExpressions;

using ErrorOr;

using Identity.Domain.Common;
using Identity.Domain.Common.Interfaces;

namespace Identity.Infrastructure.Security.PasswordHasher;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    public bool IsCorrectPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
}