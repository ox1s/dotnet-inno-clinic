namespace Identity.Domain.Common.Interfaces;

public interface IPasswordHasher
{
    public string HashPassword(string password);
    public bool IsCorrectPassword(string password, string passwordHash);
}