using FluentAssertions;

using Identity.Domain.AccountAggregate;
using Identity.Domain.Common.Interfaces;
using Identity.TestCommon.TestUtils.Services;

namespace Identity.Domain.UnitTests.AccountAggregate;

public class AccountTests
{
    private readonly IDateTimeProvider _dateTimeProvider = new TestDateTimeProvider();
    [Fact]
    public void VerifyEmail_WhenTokenIsValid_ShouldVerifyAccount()
    {
        // Arrange
        var email = Email.Create("test@test.com").Value;
        var account = Account.Create(
            email: email,
            passwordHash: "hashed_password",
            dateTimeProvider: _dateTimeProvider);
        var token = account.EmailVerificationToken;

        // Act
        var result = account.VerifyEmail(
            token: token!,
            dateTimeProvider: _dateTimeProvider);

        // Assert
        result.IsError.Should().BeFalse();
        account.IsEmailVerified.Should().BeTrue();
        account.EmailVerificationToken.Should().BeNull();
    }

    [Fact]
    public void VerifyEmail_WhenTokenIsInvalid_ShouldReturnError()
    {
        // Arrange
        var account = Account.Create(
            email: Email.Create("test@test.com").Value,
            passwordHash: "hash",
            dateTimeProvider: _dateTimeProvider);

        // Act
        var result = account.VerifyEmail(
            token: "wrong-token",
            dateTimeProvider: _dateTimeProvider);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorOr.ErrorType.Validation);
        account.IsEmailVerified.Should().BeFalse();
    }
}