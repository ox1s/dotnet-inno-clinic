using ErrorOr;

namespace Identity.Domain.AccountAggregate;

public record UpdateInfo(DateTime UpdatedAt, Guid UpdatedBy)
{
    // TODO: Implement validation
    public static ErrorOr<UpdateInfo> Create(DateTime updatedAt, Guid updatedBy)
    {
        return new UpdateInfo(updatedAt, updatedBy);
    }
}
