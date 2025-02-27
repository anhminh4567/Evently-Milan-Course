using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Users.Application.Users.GetUser;
using Evently.Modules.Users.Domain.Users.DomainEvents;
using Evently.Modules.Users.IntegrationEvents;
using MediatR;

namespace Evently.Modules.Users.Application.Users.RegisterUser;
internal class UserRegisteredDomainEventHandler : IDomainEventHandler<UserRegisteredDomainEvent>
{
    //this is replaced with eventBus
    //private readonly ITicketingApi _ticketingApi;
    private readonly IEventBus _eventBus;
    private readonly ISender _sender
        ;

    public UserRegisteredDomainEventHandler(IEventBus eventBus, ISender sender)
    {
        _eventBus = eventBus;
        _sender = sender;
    }

    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        Result<UserResponse> result = await _sender.Send(new GetUserQuery(notification.userId));
        if (result.IsFailure) 
        {
            // when this shit happen, we dont know how to handle, so throw
            throw new EventlyException(nameof(GetUserQuery),result.Error);
        }
        await _eventBus.PublishAsync(new UserRegisteredIntegrationEvent(notification.Id, notification.OccuredTimeUtc) 
        { 
            Id = notification.Id,
            OccurredOnUtc = notification.OccuredTimeUtc,
            Email = result.Value.Email,
            FirstName = result.Value.FirstName, 
            LastName = result.Value.LastName,
            UserId = result.Value.Id,
            
        },cancellationToken);
        
    }
}

