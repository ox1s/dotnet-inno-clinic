namespace Identity.Application.Common.Interfaces;

public interface IUserContext
{
    Guid UserId { get; }
    string UserRole { get; }
}