using Microsoft.AspNetCore.Mvc;

namespace Appointment.Api.Features.ListAppointment;

public record ListAppointmentsRequest(
    Guid? DoctorId,
    Guid? PatientId,
    Guid? ServiceId,
    Guid? OfficeId,
    DateOnly? Date,
    DateOnly? DateStart,
    DateOnly? DateEnd,
    bool? IsApproved,
    string? SortBy,
    string? SortDirection);
