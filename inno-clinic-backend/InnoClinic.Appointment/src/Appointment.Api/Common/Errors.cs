namespace Appointment.Api.Common;

public static class Errors
{
    public static Error ProfileNotFound =>
        new("ProfileNotFound", "Profile for this user not found");
    public static Error ProfileNotLinked =>
        new("ProfileNotLinked", "Profile is not linked to this account");
    public static Error InvalidTimeRange =>
        new("InvalidTimeRange", "Start time must be before end time");
    public static Error OverlappingAppointment =>
        new("OverlappingAppointment", "An appointment already exists for this doctor at this time.");
    public static Error TimeRangeMustBeOnSameDay =>
        new("TimeRangeMustBeOnSameDay", "The start and end time must be on the same day.");
    public static Error DoctorIsNotActive =>
        new("DoctorIsNotActive", "The doctor is not active.");
    public static Error TimeRangeOffsetMismatch =>
        new("TimeRangeOffsetMismatch", "The start and end time must have the same offset.");
    public static Error ServiceIsNotActive =>
        new("ServiceIsNotActive", "The service is not active.");
    public static Error OfficeIsNotActive =>
        new("OfficeIsNotActive", "The office is not active.");
    public static Error TimeRangeAlreadyExists =>
        new("TimeRangeAlreadyExists", "A time range already exists for this time.");
    public static Error DoctorNotFound => new("DoctorNotFound", "The doctor was not found.");
}
