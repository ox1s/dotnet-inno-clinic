using InnoClinic.Shared.DTOs;

namespace Appointment.Api.External;

public class FakeProfileGateway : IProfileGateway
{
    public Task<bool> IsDoctorActiveAsync(Guid doctorId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }

    public Task<IEnumerable<DoctorDto>> GetDoctorsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        var doctors = ids.Select(id => new DoctorDto(id, "John", "Doe", "Smith", true));
        return Task.FromResult(doctors);
    }

    public Task<IEnumerable<PatientDto>> GetPatientsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        var patients = ids.Select(id => new PatientDto(id, "Jane", "Doe", "Julia", "+1234567890", true));
        return Task.FromResult(patients);
    }
}