namespace Profile.Api.Contracts;

public sealed record CreatePatientProfileRequest(
    Guid AccountId,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime DateOfBirth,
    bool IsLinkedToAccount = true);

public sealed record CreateReceptionistProfileRequest(
    Guid AccountId,
    string FirstName,
    string LastName,
    string? MiddleName,
    Guid OfficeId);

