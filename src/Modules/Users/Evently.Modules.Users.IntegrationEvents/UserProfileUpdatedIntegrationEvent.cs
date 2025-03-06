using Evently.Common.Application.EventBus;

namespace Evently.Modules.Users.IntegrationEvents;

public sealed class UserProfileUpdatedIntegrationEvent : IntegrationEvent
{
    public UserProfileUpdatedIntegrationEvent(
        string id,
        DateTime occurredOnUtc,
        string userId,
        string firstName,
        string lastName)
        : base(id, occurredOnUtc)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
    }

    public string UserId { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }
}
