namespace Appointment.Api.Features.Receptionist.ListAppointments;

public record ListReceptionistAppointmentsResponse(
    Guid Id,
    string TimeSlot,
    string DoctorFullName,
    string PatientFullName,
    string PatientPhoneNumber,
    string ServiceName,
    bool IsApproved,
    int TotalCount
);