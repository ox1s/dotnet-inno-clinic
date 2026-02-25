using Appointment.Api.Common;

using Throw;

namespace Appointment.Api.Data;

public record TimeRange
{
    public DateTimeOffset Start { get; init; }
    public DateTimeOffset End { get; init; }

    private TimeRange(DateTimeOffset start, DateTimeOffset end)
    {
        Start = start.Throw().IfGreaterThanOrEqualTo(end);
        End = end;
    }

    public static Result<TimeRange> Create(DateTimeOffset start, DateTimeOffset end)
    {
        if (start >= end)
            return Errors.InvalidTimeRange;

        if (start.Date != end.Date)
            return Errors.TimeRangeMustBeOnSameDay;

        if (start.Offset != end.Offset)
            return Errors.TimeRangeOffsetMismatch;

        return Result<TimeRange>.Success(new TimeRange(start, end));
    }
    public TimeSpan LengthInTime => End - Start;
    private TimeRange() { }

}