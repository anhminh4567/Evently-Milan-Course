using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evently.Common.Domain;
using MediatR;

namespace Evently.Common.Application.Messaging;
public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>  where TDomainEvent: IDomainEvent
{
}
