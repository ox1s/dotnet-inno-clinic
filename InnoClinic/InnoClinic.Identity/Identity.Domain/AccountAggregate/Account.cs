using UserManagement.Core.Common;

namespace Identity.Domain.AccountAggregate;

public class Account : AggregateRoot
{
    private readonly Email _email = null;

    // TODO: Valid value: min 6 symbols, max 15 symbols
    private readonly Password _password;

       
}