namespace Appointment.Api.Data;

public class Appointment
{
    public Guid Id { get; init; }
    public Guid PatientId { get; init; }
    public Guid DoctorId { get; init; }
    public DateOnly Date { get; init; }
    public TimeRange Time { get; init; }
    public bool IsApproved { get; set; }

    private Appointment(
        Guid patientId,
        Guid doctorId,
        DateOnly date,
        TimeRange time,
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
        TimeRange time,
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
