namespace Appointment.Api.Common;

public static class Errors
{
    public static Error InvalidTimeRange => new("InvalidTimeRange", "Start time must be before end time");
}
