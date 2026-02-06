namespace Identity.Application.Common.Interfaces;

public interface IEmailVerificationLinkFactory
{
    string Create(Guid accountId, string token);
}