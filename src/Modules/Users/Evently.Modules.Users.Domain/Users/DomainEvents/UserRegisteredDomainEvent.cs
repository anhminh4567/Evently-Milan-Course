using Evently.Common.Domain;

namespace Evently.Modules.Users.Domain.Users.DomainEvents;

public sealed record UserRegisteredDomainEvent(string userId) : DomainEvent
{
    public string UserId { get; init; } = userId;
}
