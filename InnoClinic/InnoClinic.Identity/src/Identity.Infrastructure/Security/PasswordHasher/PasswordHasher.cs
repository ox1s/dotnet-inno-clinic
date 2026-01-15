using System.Text.RegularExpressions;
using ErrorOr;
using Identity.Domain.Common;

namespace Identity.Infrastructure.Security.PasswordHasher;

public partial class PasswordHasher : IPasswordHasher
{
    private static readonly Regex _passwordRegex = StrongPasswordRegex();

    public ErrorOr<string> HashPassword(string password)
    {
        return !_passwordRegex.IsMatch(password)
            ? Error.Validation(description: "Password too weak")
            : BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    public bool IsCorrectPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }

    [GeneratedRegex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,15}$", RegexOptions.Compiled)]
    private static partial Regex StrongPasswordRegex();
}
