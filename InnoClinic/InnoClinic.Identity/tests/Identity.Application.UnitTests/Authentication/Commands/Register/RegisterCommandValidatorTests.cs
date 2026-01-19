using FluentAssertions;
using Identity.Application.Authentication.Commands.Register;

namespace Identity.Application.UnitTests.Authentication.Commands.Register;

public class RegisterCommandValidatorTests
{
    private readonly RegisterCommandValidator _validator = new();

    [Theory]
    [InlineData("short", false)]
    [InlineData("NoSpecialChar1", false)]
    [InlineData("nocaps1!", false)]
    [InlineData("Valid123!", true)]
    public void Validate_Password_ShouldReturnExpectedResult(string password, bool expectedIsValid)
    {
        // Arrange
        var command = new RegisterCommand("test@test.com", password);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().Be(expectedIsValid);
    }
}
