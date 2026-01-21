namespace Appointment.Api.Appointments;

public class Appointment
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public bool IsApproved { get; set; }

    private Appointment(
        Guid patientId,
        Guid doctorId,
        DateOnly date,
        TimeOnly time,
        Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        PatientId = patientId;
        DoctorId = doctorId;
        Date = date;
        Time = time;
        IsApproved = false;
    }
    public static Appointment Create(
        Guid patientId,
        Guid doctorId,
        DateOnly date,
        TimeOnly time,
        Guid? id = null)
    {
        return new Appointment(
            patientId,
            doctorId,
            date,
            time,
            id);
    }
    public void Approve()
    {
        IsApproved = true;
    }

    private Appointment()
    {
    }
}
