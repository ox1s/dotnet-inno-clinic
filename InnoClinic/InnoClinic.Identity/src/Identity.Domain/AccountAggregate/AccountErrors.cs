using ErrorOr;

namespace Identity.Domain.AccountAggregate;

public class AccountErrors
{
    public static readonly Error AlreadyExists = Error.Conflict(
        "Account.AlreadyExists",
        "Account with this email already exists"); // Frontend: "User with this email already exists"
    public static  readonly Error AccountNotFound = Error.Conflict(
        "Account.AccountNotFound",
        "Account doesn't exists");
}
