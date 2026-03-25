using Profile.Domain.Entities.AccountProfiles;

namespace Profile.Domain.Entities.Patients;

public class Patient : AccountProfile
{
    public bool IsLinkedToAccount { get; set; }
    public DateOnly DateOfBirth { get; set; }

    private Patient(
        FirstName firstName,
        LastName lastName,
        MiddleName middleName,
        DateOnly dateOfBirth,
        Guid accountId
    ) : base(firstName, lastName, middleName, accountId)
    {
        IsLinkedToAccount = true;
        DateOfBirth = dateOfBirth;
    }

    public static Patient Create(
        FirstName firstName,
        LastName lastName,
        MiddleName middleName,
        bool isLinkedToAccount,
        DateOnly dateOfBirth,
        Guid accountId)
    {
        return new Patient(
            firstName,
            lastName,
            middleName,
            dateOfBirth,
            accountId);
    }
}