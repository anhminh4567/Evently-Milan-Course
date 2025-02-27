using MediatR;

namespace Evently.Common.Domain;

public interface IDomainEvent : INotification
{
    string Id { get; }
    DateTime OccuredTimeUtc { get; }
}
