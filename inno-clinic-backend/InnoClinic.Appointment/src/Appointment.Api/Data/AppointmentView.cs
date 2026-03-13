namespace Appointment.Api.Data;

public class AppointmentView
{
    // Фильтры
    // AC-8	The page should contain the field for filtration by doctor full name
    // AC-9 The page should contain the field for filtration by service name
    // AC-10 The page should contain the field for filtration by appointment status (Approved, Not Approved, All)
    // AC-11 The page should contain the field for filtration by office
    public Guid AppointmentId { get; set; }

    public DateTimeOffset DurationStart { get; set; }
    public DateTimeOffset DurationEnd { get; set; }
    public DateOnly LocalDate { get; set; }
    public bool IsApproved { get; set; }


    public Guid DoctorId { get; set; }
    public string DoctorFirstName { get; set; } = string.Empty;
    public string DoctorLastName { get; set; } = string.Empty;
    public string DoctorMiddleName { get; set; } = string.Empty;


    public Guid PatientId { get; set; }
    public string PatientFirstName { get; set; } = string.Empty;
    public string PatientLastName { get; set; } = string.Empty;
    public string PatientMiddleName { get; set; } = string.Empty;
    public string PatientPhone { get; set; } = string.Empty;


    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;

    public Guid OfficeId { get; set; }
    public string OfficeName { get; set; } = string.Empty;
}