using Appointment.Api.Features.Receptionist.ListAppointments;

namespace Appointment.Api.Data;

public interface IAppointmentRepository
{
    Task<Appointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Appointment appointment, CancellationToken cancellationToken = default);
    Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default);
    Task DeleteAsync(Appointment appointment, CancellationToken cancellationToken = default);
    /// <summary>
    /// True when the doctor already has an appointment overlapping <paramref name="duration"/>.
    /// Pending (not yet approved) appointments count as conflicts - otherwise the same slot
    /// could be booked any number of times while it waits for approval.
    /// </summary>
    /// <param name="excludeAppointmentId">
    /// Appointment to ignore, so rescheduling does not conflict with itself.
    /// </param>
    Task<bool> IsOverlappingAsync(
        Guid doctorId,
        TimeRange duration,
        Guid? excludeAppointmentId = null,
        CancellationToken cancellationToken = default);
    Task<int> CountAsync(AppointmentFilter filter, CancellationToken cancellationToken = default);

    Task<List<AppointmentView>> SearchAsync(AppointmentFilter filter, CancellationToken cancellationToken = default);
}