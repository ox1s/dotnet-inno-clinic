using Bogus;

using FluentAssertions;

using Identity.Application.Authentication.Commands.Register;

namespace Identity.Application.UnitTests.Authentication.Commands.Register;

public class RegisterCommandValidatorTests
{
    private readonly RegisterCommandValidator _validator = new();
    private readonly Faker _faker = new();

    [Theory]
    [InlineData("short", false)]
    [InlineData("toolongpasswordthatiswaymorethan15chars", false)]
    [InlineData("NoDigit!", false)]
    [InlineData("NoSpecial1", false)]
    [InlineData("noupper1!", false)]
    [InlineData("Valid123!", true)]
    public void Validate_Password_ShouldReturnExpectedResult(string password, bool expectedValid)
    {
        // Arrange
        var command = new RegisterCommand(_faker.Internet.Email(), password);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().Be(expectedValid);

        if (!expectedValid)
        {
            result.Errors.Should().Contain(x =>
                x.PropertyName == nameof(RegisterCommand.Password));
        }
    }
}