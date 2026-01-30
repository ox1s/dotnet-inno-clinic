namespace Appointment.Api.Features.GetAvailableSlots;

public record GetAvailableSlotsResponse(
    IEnumerable<TimeOnly> TimeSlots);
