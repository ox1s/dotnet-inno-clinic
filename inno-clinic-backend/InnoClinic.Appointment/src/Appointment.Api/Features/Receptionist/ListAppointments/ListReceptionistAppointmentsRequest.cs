namespace Appointment.Api.Features.Receptionist.ListAppointments;

public record ListReceptionistAppointmentsRequest(
    int Page = 1,
    int PageSize = 10,
    Guid? DoctorId = null,
    Guid? ServiceId = null,
    Guid? OfficeId = null,
    DateOnly? Date = null,
    bool? IsApproved = null,
    string? SortBy = null,
    string? SortDirection = "Asc"
);