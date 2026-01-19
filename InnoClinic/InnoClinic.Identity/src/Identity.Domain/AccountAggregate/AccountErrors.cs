using ErrorOr;

// using Identity.Domain.Properties;

namespace Identity.Domain.AccountAggregate;

public class AccountErrors
{
    public static readonly Error AlreadyExists = Error.Conflict(
        "Account.AlreadyExists",
        "Account with this email already exists");
    // Messages.Account_AlreadyExists);
    public static readonly Error AccountNotFound = Error.Conflict(
        "Account.AccountNotFound",
        "Account doesn't exists");
    // Messages.Account_NotFound);
    public static readonly Error AccountInactive = Error.Validation(
    "Account.Inactive",
    "Your account is inactive or suspended.");
    // Messages.Account_Inactive);
}
