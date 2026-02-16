
namespace Appointment.Api.Features.Appointments.GetAvailableSlots;

public record GetAvailableSlotsRequest(
    Guid DoctorId,
    Guid ServiceId,
    DateTimeOffset DateTime);