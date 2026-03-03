using Bogus;

using FluentAssertions;

using Identity.Application.Authentication.Commands.CreateWorkerAccount;

namespace Identity.Application.UnitTests.Authentication.Commands.CreateWorkerAccount;

public class CreateWorkerAccountCommandValidatorTests
{
    private readonly CreateWorkerAccountCommandValidator _validator = new();
    private readonly Faker _faker = new();

    [Theory]
    [InlineData("Doctor", true)]
    [InlineData("Receptionist", true)]
    [InlineData("doctor", true)]
    [InlineData("receptionist", true)]
    [InlineData("Admin", false)]
    [InlineData("Patient", false)]
    [InlineData("", false)]
    public void Validate_Role_ShouldReturnExpectedResult(string role, bool expectedValid)
    {
        // Arrange
        var command = new CreateWorkerAccountCommand(
            _faker.Internet.Email(),
            "Valid123!",
            role);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().Be(expectedValid);

        if (!expectedValid)
        {
            result.Errors.Should().Contain(x =>
                x.PropertyName == nameof(CreateWorkerAccountCommand.Role));
        }
    }
}
