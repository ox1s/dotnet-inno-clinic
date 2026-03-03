using ErrorOr;

using FluentAssertions;

using Identity.Application.Authentication.Commands.VerifyEmail;
using Identity.Application.Common.Interfaces;
using Identity.Domain.AccountAggregate;
using Identity.Domain.Common.Interfaces;
using Identity.TestCommon.TestUtils.Services;

using Microsoft.Extensions.Logging;

using NSubstitute;

namespace Identity.Application.UnitTests.Authentication.Commands.VerifyEmail;

public class VerifyEmailCommandHandlerTests
{
    private readonly IAccountsRepository _accountsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly VerifyEmailCommandHandler _handler;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<VerifyEmailCommandHandler> _logger;

    public VerifyEmailCommandHandlerTests()
    {
        _accountsRepository = Substitute.For<IAccountsRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        _logger = Substitute.For<ILogger<VerifyEmailCommandHandler>>();

        _handler = new VerifyEmailCommandHandler(
            _accountsRepository,
            _unitOfWork,
            _dateTimeProvider,
            _logger
        );
    }


    [Fact]
    public async Task Handle_WhenTokenIsValid_ShouldVerifyAccountAndCommitChanges()
    {
        // Arrange
        var creationTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        _dateTimeProvider.UtcNow.Returns(creationTime);

        var email = Email.Create("test@test.com").Value;
        var account = Account.Create(
            email: email,
            passwordHash: "hashed_password",
            dateTimeProvider: _dateTimeProvider);

        var validToken = account.EmailVerificationToken!;
        var command = new VerifyEmailCommand(account.Id, validToken);

        _accountsRepository.GetByIdAsync(
                command.AccountId,
                Arg.Any<CancellationToken>())
            .Returns(account);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        await _accountsRepository.Received(1).UpdateAsync(account, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenAccountNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new VerifyEmailCommand(
            Guid.NewGuid(),
            "any-token");

        _accountsRepository.GetByIdAsync(
                command.AccountId,
                Arg.Any<CancellationToken>())
            .Returns((Account?)null);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(AccountErrors.AccountNotFound);

        await _accountsRepository.DidNotReceive().UpdateAsync(Arg.Any<Account>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().CommitChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenTokenIsInvalid_ShouldReturnValidationErrorAndNotCommit()
    {
        // Arrange
        var creationTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        _dateTimeProvider.UtcNow.Returns(creationTime);

        var account = Account.Create(
            email: Email.Create("test@test.com").Value,
            passwordHash: "hash",
            dateTimeProvider: _dateTimeProvider);

        var command = new VerifyEmailCommand(account.Id, "invalid-token");

        _accountsRepository.GetByIdAsync(
                command.AccountId,
                Arg.Any<CancellationToken>())
            .Returns(account);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);

        await _unitOfWork.DidNotReceive().CommitChangesAsync(Arg.Any<CancellationToken>());
    }
}