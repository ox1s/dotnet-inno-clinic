using ErrorOr;

namespace Identity.Domain.AccountAggregate;

public class AccountErrors
{
    public static readonly Error AlreadyExists = Error.Conflict(
        "Account.AlreadyExists",
        Resources.Account_AlreadyExists);
    public static readonly Error AccountNotFound = Error.Conflict(
        "Account.AccountNotFound",
        Resources.Account_NotFound);
    public static readonly Error AccountInactive = Error.Validation(
        "Account.Inactive",
        Resources.Account_Inactive);
}
