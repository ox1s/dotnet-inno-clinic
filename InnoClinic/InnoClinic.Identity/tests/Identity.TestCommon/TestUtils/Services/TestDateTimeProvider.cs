using Identity.Domain.Common.Interfaces;

namespace Identity.TestCommon.TestUtils.Services;

public class TestDateTimeProvider : IDateTimeProvider
{
    private readonly DateTimeOffset? _fixedDateTime;

    public TestDateTimeProvider(DateTimeOffset? fixedDateTime = null)
    {
        _fixedDateTime = fixedDateTime;
    }

    public DateTimeOffset UtcNow => _fixedDateTime ?? DateTimeOffset.UtcNow;
}
