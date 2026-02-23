namespace ClinicManagement.Api.Specializations;

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
}
