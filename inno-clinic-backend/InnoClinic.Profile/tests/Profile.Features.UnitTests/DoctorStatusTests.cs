using FluentAssertions;

using InnoClinic.Shared;

using Profile.Domain.Entities.Doctors;

namespace Profile.UnitTests.Domain.Doctors;

/// <summary>
/// Regression cover for the Status value converter. Status.From used to return
/// "Self-Isolation" for the Statuses.SelfIsolation constant ("Self-isolation"), so the value
/// written to the status column could not be read back: the converter's read path threw
/// ArgumentException, which made the whole doctors page fail to materialise and blocked the
/// affected doctor from logging in.
/// </summary>
public class DoctorStatusTests
{
    public static TheoryData<string> AllStatuses =>
    [
        Statuses.AtWork,
        Statuses.OnVacation,
        Statuses.SickDay,
        Statuses.SickLeave,
        Statuses.SelfIsolation,
        Statuses.LeaveWithoutPay
    ];

    [Theory]
    [MemberData(nameof(AllStatuses))]
    public void From_PreservesTheCanonicalConstant(string status)
    {
        Status.From(status).Value.Should().Be(status);
    }

    [Theory]
    [MemberData(nameof(AllStatuses))]
    public void RoundTripThroughPersistence_PreservesTheStatus(string status)
    {
        var stored = Status.From(status).Value;

        Status.FromPersisted(stored).Should().Be(Status.From(status));
    }

    [Fact]
    public void FromPersisted_CanonicalizesLegacyCasing()
    {
        // Rows written before the fix contain this exact spelling.
        Status.FromPersisted("Self-Isolation").Value.Should().Be(Statuses.SelfIsolation);
    }

    [Fact]
    public void FromPersisted_DoesNotThrowOnUnknownValue()
    {
        // The read path must stay total: an unrecognised value has to leave the row readable.
        var act = () => Status.FromPersisted("something nobody expected");

        act.Should().NotThrow();
        Status.FromPersisted("something nobody expected").Value
            .Should().Be("something nobody expected");
    }

    [Fact]
    public void From_RejectsUnknownValue()
    {
        var act = () => Status.From("something nobody expected");

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CareerStartYear_FromPersisted_DoesNotThrowOnOutOfRangeValue()
    {
        var act = () => CareerStartYear.FromPersisted(1600);

        act.Should().NotThrow();
    }

    [Fact]
    public void CareerStartYear_From_RejectsOutOfRangeValue()
    {
        var act = () => CareerStartYear.From(1600);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
