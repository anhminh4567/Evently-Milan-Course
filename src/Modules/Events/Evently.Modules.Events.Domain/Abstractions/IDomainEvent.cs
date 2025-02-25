namespace Evently.Modules.Events.Domain.Abstractions;

public interface IDomainEvent
{
    string Id { get; }
    DateTime OccuredTimeUtc { get; }
}
