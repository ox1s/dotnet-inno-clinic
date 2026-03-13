using Appointment.Api.Common;

using InnoClinic.Shared.DTOs;

namespace Appointment.Api.External;

public interface IProfileGateway
{
    Task<Result<bool>> IsDoctorActiveAsync(Guid doctorId, CancellationToken cancellationToken = default);
    Task<Result<DoctorDto>> GetDoctorAsync(Guid doctorId, CancellationToken cancellationToken = default);
    Task<Result<PatientDto>> GetPatientAsync(Guid patientId, CancellationToken cancellationToken = default);
}