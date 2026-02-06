using Microsoft.AspNetCore.Mvc;

namespace Appointment.Api.Features.GetAvailableSlots;

public record GetAvailableSlotsRequest(
    Guid DoctorId,
    Guid ServiceId,
    DateTimeOffset DateTime);