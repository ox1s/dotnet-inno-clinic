using FluentAssertions;

using Identity.Application.Authentication.Commands.Register;
using Identity.Application.Common.Interfaces;
using Identity.Application.Common.Settings;
using Identity.Domain.AccountAggregate;
using Identity.Domain.Common.Interfaces;

using InnoClinic.Shared;
using InnoClinic.Shared.Contracts.Notifications;

using Microsoft.Extensions.Options;

using NSubstitute;


namespace Identity.Application.UnitTests.Authentication.Commands.Register;

public class RegisterCommandHandlerTests
{
    private readonly IAccountsRepository _accountsRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IRabbitMqService _rabbitMqService;
    private readonly RegisterCommandHandler _handler;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IEmailVerificationLinkFactory _linkFactory;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandlerTests()
    {
        var emailSettings = new EmailSettings
        {
            FromEmail = "test@innoclinic.com",
            FromName = "InnoClinic Test",
            WelcomeSubject = "Welcome!",
            WelcomeBodyTemplate = "Link: {0}"
        };

        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _accountsRepository = Substitute.For<IAccountsRepository>();
        _rabbitMqService = Substitute.For<IRabbitMqService>();
        _linkFactory = Substitute.For<IEmailVerificationLinkFactory>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();

        var options = Options.Create(emailSettings);

        _handler = new RegisterCommandHandler(
            _jwtTokenGenerator,
            _passwordHasher,
            _unitOfWork,
            _accountsRepository,
            _linkFactory,
            _dateTimeProvider,
            _rabbitMqService
            );
    }

    [Fact]
    public async Task Handle_WhenEmailIsUniqueAndValid_ShouldRegisterAndSendEmail()
    {
        // Arrange
        var command = new RegisterCommand("new@test.com", "Password123!");

        _accountsRepository.ExistsByEmailAsync(Arg.Any<Email>(), Arg.Any<CancellationToken>())
            .Returns(false);
        _passwordHasher.HashPassword(command.Password)
            .Returns("hashed_password");
        _jwtTokenGenerator.GenerateToken(Arg.Any<Account>(), Roles.Patient)
            .Returns("jwt_token");
        _linkFactory.Create(Arg.Any<Guid>(), Arg.Any<string>())
            .Returns("http://verify-link");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Token.Should().Be("jwt_token");

        await _accountsRepository.Received(1).AddAccountAsync(Arg.Is<Account>(a =>
            a.Email.Value == command.Email), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitChangesAsync(Arg.Any<CancellationToken>());
        await _rabbitMqService.Received(1).PublishAsync(new SendVerificationEmailCommand(
            Arg.Any<Guid>(), command.Email, "http://verify-link"));
    }

    [Fact]
    public async Task Handle_WhenEmailAlreadyExists_ShouldReturnConflictError()
    {
        // Arrange
        var command = new RegisterCommand("exist@test.com", "Password123!");

        _accountsRepository
            .ExistsByEmailAsync(Arg.Is<Email>(e => e.Value == command.Email),
                Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(AccountErrors.AlreadyExists);
        await _accountsRepository.DidNotReceive()
            .AddAccountAsync(Arg.Any<Account>(), Arg.Any<CancellationToken>());
        await _rabbitMqService.DidNotReceive()
            .PublishAsync(new SendVerificationEmailCommand(
                Arg.Any<Guid>(),
                Arg.Any<string>(),
                Arg.Any<string>()));
    }
}