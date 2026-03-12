namespace ClinicManagement.Api.Authorization;

public static class Permissions
{
    // US-37 Change specialization’s status & US-38 Edit specialization
    public const string SpecializationsManipulate = "specializations:manipulate"; // Receptionist
    // US-41 Create service & US-42 Change service’s status & US-43 Edit service
    public const string ServicesManipulate = "services:manipulate"; // Receptionist
    // US-39 View specialization’s info & US-40 View specializations list & US-5 View services (Patient)
    public const string SpecializationsRead = "specializations:read"; // Receptionist, Patient
    // US-44 View service’s info & US-5 View services (Patient)
    public const string ServicesRead = "services:read"; // Receptionist, Patient

    // offices
    public const string OfficesManipulate = "offices:manipulate";
}