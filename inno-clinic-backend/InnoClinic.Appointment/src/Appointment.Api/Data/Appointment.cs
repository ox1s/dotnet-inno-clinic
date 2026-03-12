
namespace Appointment.Api.Data;

public class Appointment
{
    public Guid Id { get; init; }
    public Guid PatientId { get; init; }
    public Guid DoctorId { get; private set; }
    public Guid ServiceId { get; private set; }
    public Guid OfficeId { get; private set; }
    public TimeRange Duration { get; private set; } = null!;
    public bool IsApproved { get; set; }
    private Appointment(
        Guid patientId,
        Guid doctorId,
        Guid serviceId,
        Guid officeId,
        TimeRange duration,
        Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        PatientId = patientId;
        DoctorId = doctorId;
        ServiceId = serviceId;
        OfficeId = officeId;
        Duration = duration;
        IsApproved = false;
    }
    private Appointment() { } // EF Core
    public static Appointment Create(
        Guid patientId,
        Guid doctorId,
        Guid serviceId,
        Guid officeId,
        TimeRange duration,
        Guid? id = null)
    {
        var timeResult = TimeRange.Create(duration.Start, duration.End);
        if (!timeResult.Succeeded)
            throw new InvalidOperationException($"Invalid time range: {timeResult.Error.Code}");

        return new Appointment(
            patientId,
            doctorId,
            serviceId,
            officeId,
            timeResult.Value!,
            id);
    }

    public void Update(
        Guid doctorId,
        Guid serviceId,
        Guid officeId,
        TimeRange duration)
    {
        DoctorId = doctorId;
        ServiceId = serviceId;
        OfficeId = officeId;
        Duration = duration;

        var timeResult = TimeRange.Create(duration.Start, duration.End);
        if (!timeResult.Succeeded)
            throw new InvalidOperationException($"Invalid time range: {timeResult.Error.Code}");

        Duration = timeResult.Value!;

        IsApproved = false;
    }

    public void Approve()
    {
        IsApproved = true;
    }
}