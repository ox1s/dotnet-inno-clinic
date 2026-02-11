namespace Appointment.Api.Data;

public interface IAppointmentRepository
{
    Task<Appointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Appointment appointment, CancellationToken cancellationToken = default);
    Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default);
    Task DeleteAsync(Appointment appointment, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid DoctorId, TimeRange duration, CancellationToken cancellationToken = default);
}