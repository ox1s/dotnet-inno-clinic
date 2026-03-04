using Profile.Domain.Abstractions;
using Profile.Domain.Entities.AccountProfiles;

namespace Profile.Domain.Entities.Receptionists;

public class Receptionist : AccountProfile
{
    public Guid OfficeId { get; set; }

    private Receptionist(
        FirstName firstName,
        LastName lastName,
        MiddleName middleName,
        Guid accountId,
        Guid officeId
    ) : base(firstName, lastName, middleName, accountId)
    {
        OfficeId = officeId;
    }
    public static Receptionist Create(
        FirstName firstName,
        LastName lastName,
        MiddleName middleName,
        Guid accountId,
        Guid officeId)
    {
        return new Receptionist(
            firstName,
            lastName,
            middleName,
            accountId,
            officeId);
    }
}