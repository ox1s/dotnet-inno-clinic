using Appointment.Api.Common;
using Throw;

namespace Appointment.Api.Data;

public record TimeRange
{
    public TimeOnly Start { get; init; }
    public TimeOnly End { get; init; }

    private TimeRange(TimeOnly start, TimeOnly end)
    {
        Start = start.Throw().IfGreaterThanOrEqualTo(end);
        End = end;
    }

    public static Result<TimeRange> Create(TimeOnly start, TimeOnly end)
    {
        if (start >= end)
            return Errors.InvalidTimeRange;

        return Result<TimeRange>.Success(new TimeRange(start, end));
    }

    public bool Overlaps(TimeRange other)
    {
        return Start <= other.End && End >= other.Start;
    }
}
