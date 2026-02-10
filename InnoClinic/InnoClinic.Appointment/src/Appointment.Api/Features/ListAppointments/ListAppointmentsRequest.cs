namespace Appointment.Api.Features.ListAppointments;

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