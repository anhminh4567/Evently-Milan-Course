namespace Evently.Common.Domain;

public interface IDomainEvent
{
    string Id { get; }
    DateTime OccuredTimeUtc { get; }
}
