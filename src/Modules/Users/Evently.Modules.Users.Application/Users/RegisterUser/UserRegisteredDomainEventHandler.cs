using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evently.Common.Application.Exceptions;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Ticketing.PublicApi;
using Evently.Modules.Users.Application.Users.GetUser;
using Evently.Modules.Users.Domain.Users.DomainEvents;
using MediatR;

namespace Evently.Modules.Users.Application.Users.RegisterUser;
internal class UserRegisteredDomainEventHandler : IDomainEventHandler<UserRegisteredDomainEvent>
{
    private readonly ITicketingApi _ticketingApi;
    private readonly ISender _sender;

    public UserRegisteredDomainEventHandler(ITicketingApi ticketingApi, ISender sender)
    {
        _ticketingApi = ticketingApi;
        _sender = sender;
    }

    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        Result<UserResponse> result = await _sender.Send(new GetUserQuery(notification.Id));
        if (result.IsFailure) 
        {
            // when this shit happen, we dont know how to handle, so throw
            throw new EventlyException(nameof(GetUserQuery),result.Error);
        }
        await _ticketingApi.CreateCustomerAsync(result.Value.Id, result.Value.Email, result.Value.FirstName, result.Value.LastName);
    }
}

