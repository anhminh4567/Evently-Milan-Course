using Evently.Common.Application.EventBus;

namespace Evently.Modules.Users.IntegrationEvents;

public class UserRegisteredIntegrationEvent : IntegrationEvent
{
    public UserRegisteredIntegrationEvent(string id, DateTime occurredOnUtc) : base(id, occurredOnUtc)
    {
    }
    public string UserId {  get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
