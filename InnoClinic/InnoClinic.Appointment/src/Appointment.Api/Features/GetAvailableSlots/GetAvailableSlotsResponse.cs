namespace Appointment.Api.Features.GetAvailableSlots;

public record GetAvailableSlotsResponse(
    IEnumerable<DateTimeOffset> TimeSlots);