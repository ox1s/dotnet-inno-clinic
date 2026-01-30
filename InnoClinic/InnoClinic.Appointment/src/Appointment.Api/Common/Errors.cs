namespace Appointment.Api.Common;

public static class Errors
{
    public static Error InvalidTimeRange => new("InvalidTimeRange", "Start time must be before end time");
    public static Error OverlappingAppointment => new("OverlappingAppointment", "An appointment already exists for this doctor at this time.");

    public static Error DoctorIsNotActive => new("DoctorIsNotActive", "The doctor is not active.");

    public static Error ServiceIsNotActive => new("ServiceIsNotActive", "The service is not active.");
    public static Error OfficeIsNotActive => new("OfficeIsNotActive", "The office is not active.");
}
