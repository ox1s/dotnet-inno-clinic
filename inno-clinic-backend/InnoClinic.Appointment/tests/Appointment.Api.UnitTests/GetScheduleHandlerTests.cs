using Appointment.Api.Common;
using Appointment.Api.Data;
using Appointment.Api.Features.Doctor.GetSchedule;
using Appointment.Api.Features.Receptionist.ListAppointments;

using Moq;

namespace Appointment.Api.UnitTests;

public class GetScheduleHandlerTests
{
    [Fact]
    public async Task Handle_WhenDoctorIdIsNull_ShouldReturnUnauthorized()
    {
        // Arrange
        var mockRepository = new Mock<IAppointmentRepository>();
        var mockUserProvider = new Mock<ICurrentUserProvider>();
        mockUserProvider.Setup(x => x.GetUserId()).Returns((Guid?)null);

        // Act
        var result = await GetScheduleHandler.Handle(
            date: null,
            repository: mockRepository.Object,
            currentUserProvider: mockUserProvider.Object,
            cancellationToken: CancellationToken.None);

        // Assert
        Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.UnauthorizedHttpResult>(result);
    }

    [Fact]
    public async Task Handle_WhenDoctorIdExists_ShouldReturnSchedule()
    {
        // Arrange
        var doctorId = Guid.NewGuid();
        var targetDate = DateOnly.FromDateTime(DateTime.UtcNow);

        var mockRepository = new Mock<IAppointmentRepository>();
        var mockUserProvider = new Mock<ICurrentUserProvider>();

        mockUserProvider.Setup(x => x.GetUserId()).Returns(doctorId);

        var appointments = new List<AppointmentView>
        {
            new() {
                AppointmentId = Guid.NewGuid(),
                DurationStart = DateTimeOffset.UtcNow,
                DurationEnd = DateTimeOffset.UtcNow.AddMinutes(30),
                PatientId = Guid.NewGuid(),
                PatientFirstName = "John",
                PatientLastName = "Doe",
                PatientMiddleName = "M",
                ServiceName = "Consultation",
                IsApproved = true
            }
        };

        mockRepository
            .Setup(x => x.SearchAsync(It.IsAny<AppointmentFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointments);

        // Act
        var result = await GetScheduleHandler.Handle(
            date: targetDate,
            repository: mockRepository.Object,
            currentUserProvider: mockUserProvider.Object,
            cancellationToken: CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<List<GetScheduleHandler.Response>>>(result);
        Assert.Single(okResult.Value!);
        Assert.Equal("Doe John M", okResult.Value![0].PatientFullName);
        Assert.Equal("Consultation", okResult.Value![0].ServiceName);
        Assert.True(okResult.Value![0].IsApproved);
    }

    [Fact]
    public async Task Handle_WhenNoDateProvided_ShouldUseCurrentDate()
    {
        // Arrange
        var doctorId = Guid.NewGuid();
        var mockRepository = new Mock<IAppointmentRepository>();
        var mockUserProvider = new Mock<ICurrentUserProvider>();

        mockUserProvider.Setup(x => x.GetUserId()).Returns(doctorId);
        mockRepository
            .Setup(x => x.SearchAsync(It.IsAny<AppointmentFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        await GetScheduleHandler.Handle(
            date: null,
            repository: mockRepository.Object,
            currentUserProvider: mockUserProvider.Object,
            cancellationToken: CancellationToken.None);

        // Assert
        mockRepository.Verify(x => x.SearchAsync(
            It.Is<AppointmentFilter>(f => f.Date == DateOnly.FromDateTime(DateTime.UtcNow)),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
