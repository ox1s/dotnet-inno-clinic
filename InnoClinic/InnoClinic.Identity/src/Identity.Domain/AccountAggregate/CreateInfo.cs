using ErrorOr;

namespace Identity.Domain.AccountAggregate;

public record CreateInfo(DateTime CreatedAt, Guid CreatedBy)
{
    // TODO: Implement validation
    public static ErrorOr<CreateInfo> Create(DateTime createdAt, Guid createdBy)
    {
        return new CreateInfo(createdAt, createdBy);
    }


}
