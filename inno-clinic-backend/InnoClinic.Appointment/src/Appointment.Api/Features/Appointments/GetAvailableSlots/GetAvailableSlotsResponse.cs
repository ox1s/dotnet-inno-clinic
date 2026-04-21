namespace Appointment.Api.Features.Appointments.GetAvailableSlots;

public record GetAvailableSlotsResponse(
    IEnumerable<DateTimeOffset> TimeSlots);