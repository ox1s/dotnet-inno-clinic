using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Profile.Domain.Entities.AccountProfiles;
using Profile.Domain.Entities.Doctors;
using Profile.Features.Doctors.EditDoctorStatusByBot;
using Profile.Infrastructure.Database;
using Profile.Infrastructure.Database.Repositories;

using Xunit;

namespace Profile.UnitTests.Features.Doctors;

public class EditDoctorStatusByBotCommandHandlerTests
{
    private readonly ProfileDbContext _dbContext;
    private readonly DoctorRepository _doctorRepository;
    private readonly AccountRepository _accountProfileRepository;

    public EditDoctorStatusByBotCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ProfileDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ProfileDbContext(options);
        _doctorRepository = new DoctorRepository(_dbContext);
        _accountProfileRepository = new AccountRepository(_dbContext);
    }

    private Doctor CreateTestDoctor(Guid accountId, string initialStatus)
    {
        return Doctor.Create(
            new FirstName("Иван"),
            new LastName("Иванов"),
            new MiddleName("Иванович"),
            new DateOnly(1985, 5, 20),
            accountId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            CareerStartYear.From(2010),
            Status.From(initialStatus)
        );
    }

    [Fact]
    public async Task Handle_ShouldUpdateDoctorStatus_WhenDoctorExists()
    {
        // Arrange
        var accountId = Guid.NewGuid();

        var initialDoctor = CreateTestDoctor(accountId, "On vacation");

        _dbContext.Set<Doctor>().Add(initialDoctor);
        await _dbContext.SaveChangesAsync();

        var command = new EditDoctorStatusByBotCommand(accountId, "At work");

        // Act
        await EditDoctorStatusByBotCommandHandler.Handle(command, _dbContext, _doctorRepository, _accountProfileRepository);

        // Assert
        var updatedDoctor = await _dbContext.Set<Doctor>().FirstOrDefaultAsync(d => d.AccountId == accountId);

        updatedDoctor.Should().NotBeNull();

        updatedDoctor!.Status.Value.Should().Be("At work");
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenDoctorDoesNotExist()
    {
        // Arrange
        var nonExistentDoctorId = Guid.NewGuid();
        var command = new EditDoctorStatusByBotCommand(nonExistentDoctorId, "At work");

        // Act
        var act = async () => await EditDoctorStatusByBotCommandHandler.Handle(command, _dbContext, _doctorRepository, _accountProfileRepository);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage($"*not found*");
    }
}