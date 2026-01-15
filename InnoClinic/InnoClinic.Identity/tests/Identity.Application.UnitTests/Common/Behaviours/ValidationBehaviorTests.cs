using ErrorOr;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using NSubstitute;

using Identity.Application.Authentication.Commands.Register;
using Identity.Application.Authentication.Common;
using Identity.Application.Common.Behaviours;
using Identity.TestCommon.Authentication;

namespace Identity.Application.UnitTests.Common.Behaviours;

public class ValidationBehaviorTests
{
    private readonly RequestHandlerDelegate<ErrorOr<AuthenticationResult>> _mockNextBehavior;
    private readonly IValidator<RegisterCommand> _mockValidator;
    private readonly ValidationBehavior<RegisterCommand, ErrorOr<AuthenticationResult>> _validationBehavior;

    public ValidationBehaviorTests()
    {
        _mockValidator = Substitute.For<IValidator<RegisterCommand>>();
        _mockNextBehavior = Substitute.For<RequestHandlerDelegate<ErrorOr<AuthenticationResult>>>();
        _validationBehavior = new ValidationBehavior<RegisterCommand, ErrorOr<AuthenticationResult>>(_mockValidator);
    }

    [Fact]
    public async Task InvokeBehavior_WhenValidatorResultIsNotValid_ShouldReturnListOfErrors()
    {
        // Arrange
        var registerRequest = AccountCommandFactory.CreateCreateAccountCommand();
        List<ValidationFailure> validationFailures = [new("фука", "фука случилась")];

        _mockValidator
             .ValidateAsync(registerRequest, Arg.Any<CancellationToken>())
             .Returns(new ValidationResult(validationFailures));

        // Act
        var result = await _validationBehavior.Handle(registerRequest, _mockNextBehavior, default);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("фука");
        result.FirstError.Description.Should().Be("фука случилась");
    }
    // [Fact]
    // public async Task InvokeBehavior_WhenValidatorResultIsValid_ShouldInvokeNextBehavior()
    // {
    // }
}
