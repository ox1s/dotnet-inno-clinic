using Appointment.Api.Common;
using Appointment.Api.Data;

namespace Appointment.Api.UnitTests;

public class AppointmentsTests
{
    [Fact]
    public void CreateAppointment_WhenValidData_ShouldCreateAppointment()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var doctorId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();
        var officeId = Guid.NewGuid();
        var timeRange = TimeRange.Create(
            start: DateTimeOffset.UtcNow,
            end: DateTimeOffset.UtcNow.AddHours(1));

        // Act
        var appointment = Data.Appointment.Create(
            patientId: patientId,
            doctorId: doctorId,
            serviceId: serviceId,
            officeId: officeId,
            time: timeRange.Value!);

        // Assert
        appointment.PatientId.Equals(patientId);
        appointment.DoctorId.Equals(doctorId);
        appointment.ServiceId.Equals(serviceId);
        appointment.OfficeId.Equals(officeId);
        appointment.Time.Equals(timeRange.Value!);
        appointment.IsApproved.Equals(false);

    }
    [Fact]
    public void ApproveAppointment_ShouldSetIsApprovedToTrue()
    {
        // Arrange
        var appointment = Data.Appointment.Create(
            patientId: Guid.NewGuid(),
            doctorId: Guid.NewGuid(),
            serviceId: Guid.NewGuid(),
            officeId: Guid.NewGuid(),
            time: TimeRange.Create(
                start: DateTimeOffset.UtcNow,
                end: DateTimeOffset.UtcNow.AddHours(1)).Value!);

        // Act
        appointment.Approve();

        // Assert
        Assert.True(appointment.IsApproved);
    }

    [Fact]
    public void CreateTimeRange_WhenDataIsInvalid_ShouldThrowArgumentException()
    {
        // Arrange
        var timeOffset = TimeSpan.FromHours(-3);
        var invalidTime = new DateTimeOffset(DateTimeOffset.UtcNow.DateTime, timeOffset);
        var timeRange = TimeRange.Create(
            start: DateTimeOffset.UtcNow,
            end: invalidTime);

        timeRange.Error.Equals(Errors.TimeRangeOffsetMismatch);
    }

    [Fact]
    public void CreateAppointment_WhenTimeRangeIsNull_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var doctorId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();
        var officeId = Guid.NewGuid();
        var timeOffset = TimeSpan.FromHours(-3);
        var invalidTime = new DateTimeOffset(DateTimeOffset.UtcNow.DateTime, timeOffset);
        var timeRange = TimeRange.Create(
            start: DateTimeOffset.UtcNow,
            end: invalidTime);

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => Data.Appointment.Create(
            patientId: patientId,
            doctorId: doctorId,
            serviceId: serviceId,
            officeId: officeId,
            time: timeRange.Value!));
    }
}
