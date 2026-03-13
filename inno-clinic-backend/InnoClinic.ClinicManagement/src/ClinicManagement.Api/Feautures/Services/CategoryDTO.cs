namespace ClinicManagement.Api.Features.Services;

internal sealed partial class GetService
{
    public sealed record CategoryDTO(Guid Id, string Name);
}