using Identity.Application.Authentication.Commands.Register;
using Identity.TestCommon.AccountAggregate;

namespace Identity.TestCommon.Authentication;

public class AccountCommandFactory
{
    public static RegisterCommand CreateCreateAccountCommand(
        string? email = null,
        string? password = null)
    {
        return new RegisterCommand(
            email ?? Constants.Account.Email.Value,
            password ?? Constants.Account.PasswordHash);
    }

}