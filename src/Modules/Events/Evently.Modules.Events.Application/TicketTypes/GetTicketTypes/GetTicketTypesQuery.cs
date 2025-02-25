using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Application.TicketTypes.GetTicketType;

namespace Evently.Modules.Events.Application.TicketTypes.GetTicketTypes;

public sealed record GetTicketTypesQuery(string EventId) : IQuery<IReadOnlyCollection<TicketTypeResponse>>;
