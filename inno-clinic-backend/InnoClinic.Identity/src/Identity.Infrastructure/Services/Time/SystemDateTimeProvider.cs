using Identity.Domain.Common.Interfaces;

namespace Identity.Infrastructure.Services.Time;

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}