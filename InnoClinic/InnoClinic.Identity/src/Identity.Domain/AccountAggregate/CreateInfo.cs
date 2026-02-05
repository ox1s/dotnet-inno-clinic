using ErrorOr;

namespace Identity.Domain.AccountAggregate;

public record CreateInfo(DateTimeOffset CreatedAt, Guid  CreatedBy)
{
    // TODO: Implement validation
    public static ErrorOr<CreateInfo> Create(DateTimeOffset createdAt, Guid createdBy)
    {
        return new CreateInfo(createdAt, createdBy);
    }


}
