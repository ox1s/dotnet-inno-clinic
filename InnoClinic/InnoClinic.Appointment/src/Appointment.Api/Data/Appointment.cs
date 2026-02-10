
namespace Appointment.Api.Data;

public class Appointment
{
    public Guid Id { get; init; }
    public Guid PatientId { get; init; }
    public Guid DoctorId { get; private set; }
    public Guid ServiceId { get; private set; }
    public Guid OfficeId { get; private set; }
    public DateOnly LocalDate => DateOnly.FromDateTime(Time.Start.Date);
    public TimeRange Time { get; private set; } = null!;

    public Guid TimeRangeId { get; private set; }
    public bool IsApproved { get; set; }

    private Appointment(
        Guid patientId,
        Guid doctorId,
        Guid serviceId,
        Guid officeId,
        TimeRange time,
        Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        PatientId = patientId;
        DoctorId = doctorId;
        ServiceId = serviceId;
        OfficeId = officeId;
        Time = time;
        IsApproved = false;
    }
    public static Appointment Create(
        Guid patientId,
        Guid doctorId,
        Guid serviceId,
        Guid officeId,
        TimeRange time,
        Guid? id = null)
    {
        var timeResult = TimeRange.Create(time.Start, time.End);
        if (timeResult.Succeeded is false)
            throw new InvalidOperationException($"Invalid time range: {timeResult.Error.Code}");

        return new Appointment(
            patientId,
            doctorId,
            serviceId,
            officeId,
            time,
            id);
    }

    public void Update(
        Guid doctorId,
        Guid serviceId,
        Guid officeId,
        TimeRange time)
    {
        DoctorId = doctorId;
        ServiceId = serviceId;
        OfficeId = officeId;

        var timeResult = TimeRange.Create(time.Start, time.End);
        if (timeResult.Succeeded is false)
            throw new InvalidOperationException($"Invalid time range: {timeResult.Error.Code}");

        Time = timeResult.Value!;

        IsApproved = false;
    }

    public void Approve()
    {
        IsApproved = true;
    }

    private Appointment() { }
}