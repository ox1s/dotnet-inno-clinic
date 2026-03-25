namespace ClinicManagement.Api.Data.Entities;

public class Specialization
{
    // id(PK)
    // specialization_name
    // isActive

    public Guid Id { get; set; }
    public string SpecializationName { get; set; } = null!;
    public bool IsActive { get; set; }

    public static Specialization Create(string specializationName, bool isActive)
    {
        return new Specialization
        {
            SpecializationName = specializationName,
            IsActive = isActive,
        };
    }

    public void Update(string specializationName, bool isActive)
    {
        SpecializationName = specializationName;
        IsActive = isActive;
    }

    public void ChangeStatus(bool isActive)
    {
        IsActive = isActive;
    }
    private Specialization() { } // EF Core
}
