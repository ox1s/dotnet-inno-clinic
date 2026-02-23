using InnoClinic.Shared;

namespace ClinicManagement.Api.Authorization;

public static class RolePermissionMapping
{
    public static readonly Dictionary<string, HashSet<string>> Map = new()
    {
        {
            Roles.Receptionist, new HashSet<string>
            {
                Permissions.ServicesManipulate,
                Permissions.SpecializationsManipulate,
                Permissions.ServicesRead,
                Permissions.SpecializationsRead
            }
        },
        {
            Roles.Patient, new HashSet<string>
            {
                Permissions.ServicesRead,
                Permissions.SpecializationsRead
            }
        }
    };
}