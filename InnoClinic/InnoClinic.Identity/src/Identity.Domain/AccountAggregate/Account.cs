using ErrorOr;

using Identity.Domain.Common;
using Identity.Domain.Common.Interfaces;
namespace Identity.Domain.AccountAggregate;

public class Account : AggregateRoot
{
    public Email Email { get; private set; } = null!;
    public bool IsEmailVerified { get; private set; }
    public string? EmailVerificationToken { get; private set; }
    public DateTimeOffset? EmailVerificationTokenExpiration { get; private set; }
    public PhoneNumber? PhoneNumber { get; private set; }
    public Guid? PhotoId { get; private set; }
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
    }
    public static Account Create(
        Email email,
        string passwordHash,
        IDateTimeProvider dateTimeProvider)
    {
        var account = new Account(
            email: email,
            passwordHash: passwordHash);
        var now = dateTimeProvider.UtcNow;
        account.CreatedInfo = new CreateInfo(dateTimeProvider.UtcNow, account.Id);
        account.EmailVerificationTokenExpiration = now.AddDays(1);
        return account;
    }
    public ErrorOr<Account> Update(
        Guid? photoId,
        PhoneNumber? phoneNumber)
    {
        PhotoId = photoId ?? PhotoId;
        PhoneNumber = phoneNumber ?? PhoneNumber;
        return this;
    }
    public ErrorOr<Success> VerifyEmail(string token, IDateTimeProvider dateTimeProvider)
    {
        if (IsEmailVerified)
            return Error.Conflict(description: "Email already verified");
        if (EmailVerificationToken != token)
            return Error.Validation(description: "Invalid token");
        if (dateTimeProvider.UtcNow > EmailVerificationTokenExpiration)
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