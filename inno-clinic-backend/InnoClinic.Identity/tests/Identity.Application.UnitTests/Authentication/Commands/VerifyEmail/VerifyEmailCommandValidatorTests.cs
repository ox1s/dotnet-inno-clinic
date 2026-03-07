using FluentAssertions;

using Identity.Application.Authentication.Commands.VerifyEmail;

namespace Identity.Application.UnitTests.Authentication.Commands.VerifyEmail;

public class VerifyEmailCommandValidatorTests
{
    private readonly VerifyEmailCommandValidator _validator = new();

    [Theory]
    [InlineData("notEmpty", true)]
    [InlineData("", false)]
    public void Validate_Token_ShouldReturnExpectedResult(string token, bool expectedValid)
    {
        // Arrange
        var command = new VerifyEmailCommand(
            Guid.NewGuid(),
            token ?? ""
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().Be(expectedValid);

        if (!expectedValid)
        {
            result.Errors.Should().Contain(x =>
                x.PropertyName == nameof(VerifyEmailCommand.Token));
        }
    }
}