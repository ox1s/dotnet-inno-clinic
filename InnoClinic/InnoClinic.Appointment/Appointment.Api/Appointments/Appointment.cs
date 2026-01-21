namespace Appointment.Api.Appointments;

public class Appointment
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public bool IsApproved { get; set; }

    public Appointment(Guid patientId,
        Guid doctorId,
        DateOnly date,
        TimeOnly time)
    {
        Id = Guid.NewGuid();
        PatientId = patientId;
        DoctorId = doctorId;
        Date = date;
        Time = time;
        IsApproved = false;
    }

    public void Approve()
    {
        IsApproved = true;
    }

    private Appointment()
    {
    }
}
