using System.Collections.Immutable;

using InnoClinic.Shared;

namespace ClinicManagement.Api.Authorization;

public static class RolePermissionMapping
{
    public static readonly ImmutableDictionary<string, ImmutableHashSet<string>> Map =
        new Dictionary<string, ImmutableHashSet<string>>
        {
            {
                Roles.Receptionist, ImmutableHashSet.Create(
                    Permissions.SpecializationsManipulate,
                    Permissions.SpecializationsRead,
                    Permissions.OfficesManipulate)
            },
            {
                Roles.Patient, ImmutableHashSet.Create(
                    Permissions.SpecializationsRead)
            }
        }.ToImmutableDictionary();
}