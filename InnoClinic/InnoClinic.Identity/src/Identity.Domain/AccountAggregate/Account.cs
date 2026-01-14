using ErrorOr;

using Identity.Domain.Common;

namespace Identity.Domain.AccountAggregate;

public class Account : AggregateRoot
{
    public Email Email { get; private set; } = null!;
    public bool IsEmailVerified { get; private set; }
    public string? EmailVerificationToken { get; private set; }
    public DateTime? EmailVerificationTokenExpiration { get; private set; }

    public Guid PhotoId { get; private set; }
    public CreateInfo CreatedInfo { get; private set; } = null!;
    public UpdateInfo? UpdatedInfo { get; private set; }

    private readonly string _passwordHash = null!;

    private Account(
        Email email,
        string passwordHash,
        Guid? id = null)
        : base(id ?? Guid.NewGuid())
    {
        Email = email;
        _passwordHash = passwordHash;
        IsEmailVerified = false;

        EmailVerificationToken = Guid.NewGuid().ToString();
        EmailVerificationTokenExpiration = DateTime.UtcNow.AddDays(1);
    }

    public static Account Create(
        Email email,
        string passwordHash)
    {
        var account = new Account(
            email: email,
            passwordHash: passwordHash);

        account.CreatedInfo = new CreateInfo(DateTime.UtcNow, account.Id);

        // account.DomainEvents.Add(new AccountCreatedDomainEvent(account.Id));

        return account;
    }

    public ErrorOr<Success> VerifyEmail(string token)
    {
        if (IsEmailVerified) return Error.Conflict(description: "Email already verified");

        if (EmailVerificationToken != token) return Error.Validation(description: "Invalid token");

        if (DateTime.UtcNow > EmailVerificationTokenExpiration)
            return Error.Validation(description: "Token expired");

        IsEmailVerified = true;
        EmailVerificationToken = null;
        EmailVerificationTokenExpiration = null;

        return Result.Success;
    }
    public bool IsCorrectPasswordHash(string password, IPasswordHasher passwordHasher)
    {
        return passwordHasher.IsCorrectPassword(password, _passwordHash);
    }

    private Account() { }
}
