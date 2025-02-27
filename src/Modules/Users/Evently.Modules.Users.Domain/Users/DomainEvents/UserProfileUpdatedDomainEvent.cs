using Evently.Common.Domain;

namespace Evently.Modules.Users.Domain.Users.DomainEvents;

public sealed record UserProfileUpdatedDomainEvent(string userId, string firstName, string lastName) : DomainEvent
{
    public string UserId { get; init; } = userId;

    public string FirstName { get; init; } = firstName;

    public string LastName { get; init; } = lastName;
}
