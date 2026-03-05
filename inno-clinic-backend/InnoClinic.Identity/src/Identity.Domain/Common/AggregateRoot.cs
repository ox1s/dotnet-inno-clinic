namespace Identity.Domain.Common;

public abstract class AggregateRoot : Entity
{
    protected AggregateRoot(Guid id) : base(id)
    {
    }

    protected AggregateRoot() { }

    // TODO: Domain Events
}