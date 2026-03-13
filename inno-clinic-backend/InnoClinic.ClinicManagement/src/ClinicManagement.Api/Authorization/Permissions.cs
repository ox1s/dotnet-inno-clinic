namespace ClinicManagement.Api.Authorization;

public static class Permissions
{
    public const string SpecializationsManipulate = "specializations:manipulate"; // Receptionist
    public const string SpecializationsRead = "specializations:read"; // Receptionist, Patient
    // offices
    public const string OfficesManipulate = "offices:manipulate";
}