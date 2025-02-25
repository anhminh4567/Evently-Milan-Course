using Evently.Modules.Events.Application.Abstractions.Messaging;

namespace Evently.Modules.Events.Application.TicketTypes.GetTicketType;

public sealed record GetTicketTypeQuery(string TicketTypeId) : IQuery<TicketTypeResponse>;
