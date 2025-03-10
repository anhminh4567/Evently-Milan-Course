using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evently.Common.Domain;
using MediatR;

namespace Evently.Common.Application.Messaging;
public interface IDomainEventHandler   
{
    Task Handle(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
public interface IDomainEventHandler<in TDomainEvent> : IDomainEventHandler 
    where TDomainEvent : IDomainEvent //: INotificationHandler<TDomainEvent>  where TDomainEvent: IDomainEvent
{
    Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
