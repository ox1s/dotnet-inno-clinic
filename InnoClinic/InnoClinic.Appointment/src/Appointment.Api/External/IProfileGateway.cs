using InnoClinic.Shared.DTOs;

namespace Appointment.Api.External;

public interface IProfileGateway
{
    Task<bool> IsDoctorActiveAsync(Guid doctorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DoctorDto>> GetDoctorsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    Task<IEnumerable<PatientDto>> GetPatientsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
}