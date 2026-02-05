using ErrorOr;

namespace Identity.Domain.AccountAggregate;

public record UpdateInfo(DateTimeOffset UpdatedAt, Guid UpdatedBy)
{
    // TODO: Implement validation
    public static ErrorOr<UpdateInfo> Create(DateTimeOffset updatedAt, Guid updatedBy)
    {
        return new UpdateInfo(updatedAt, updatedBy);
    }
}
