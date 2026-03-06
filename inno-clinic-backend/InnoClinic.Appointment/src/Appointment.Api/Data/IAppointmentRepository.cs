namespace Appointment.Api.Data;

public interface IAppointmentRepository
{
    Task<Appointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Appointment appointment, CancellationToken cancellationToken = default);
    Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default);
    Task DeleteAsync(Appointment appointment, CancellationToken cancellationToken = default);
    Task<bool> IsOverlappingAsync(Guid doctorId, TimeRange duration, CancellationToken cancellationToken = default);
    // Task<List<Appointment>> SearchAsync(AppointmentFilter filter, CancellationToken cancellationToken = default);
    // Task<int> CountAsync(AppointmentFilter filter, CancellationToken cancellationToken = default);

    // Task<List<AppointmentView>> SearchAsync(AppointmentFilter filter, CancellationToken cancellationToken = default);
    // Task<List<AppointmentView>> SearchAsync(AppointmentFilter filter, CancellationToken cancellationToken = default);
}