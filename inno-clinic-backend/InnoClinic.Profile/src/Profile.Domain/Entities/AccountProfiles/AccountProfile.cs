using Profile.Domain.Abstractions;

namespace Profile.Domain.Entities.AccountProfiles;

public abstract class AccountProfile : Entity
{
    public Guid AccountId { get; set; }
    public FirstName FirstName { get; set; } = null!;
    public LastName LastName { get; set; } = null!;
    public MiddleName MiddleName { get; set; } = null!;

    protected AccountProfile(FirstName firstName, LastName lastName, MiddleName middleName, Guid accountId, Guid? id = null)
        : base(id ?? Guid.NewGuid())
    {
        AccountId = accountId;
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }
}