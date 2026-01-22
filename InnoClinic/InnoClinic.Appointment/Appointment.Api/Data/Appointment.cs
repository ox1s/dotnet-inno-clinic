namespace Appointment.Api.Data;

public class Appointment
{
    public Guid Id { get; init; }
    public Guid PatientId { get; init; }
    public Guid DoctorId { get; private set; }
    public Guid ServiceId { get; private set; }
    public Guid OfficeId { get; private set; }
    public DateOnly Date { get; private set; }
    public TimeRange Time { get; private set; }
    public bool IsApproved { get; set; }

    private Appointment(
        Guid patientId,
        Guid doctorId,
        Guid serviceId,
        Guid officeId,
        DateOnly date,
        TimeRange time,
        Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        PatientId = patientId;
        DoctorId = doctorId;
        ServiceId = serviceId;
        OfficeId = officeId;
        Date = date;
        Time = time;
        IsApproved = false;
    }
    public static Appointment Create(
        Guid patientId,
        Guid doctorId,
        Guid serviceId,
        Guid officeId,
        DateOnly date,
        TimeRange time,
        Guid? id = null)
    {
        return new Appointment(
            patientId,
            doctorId,
            serviceId,
            officeId,
            date,
            time,
            id);
    }

    public void Update(
        Guid doctorId,
        Guid serviceId,
        Guid officeId,
        DateOnly date,
        TimeRange time)
    {
        DoctorId = doctorId;
        ServiceId = serviceId;
        OfficeId = officeId;
        Date = date;
        Time = time;

        IsApproved = false;
    }

    public void Approve()
    {
        IsApproved = true;
    }
}
