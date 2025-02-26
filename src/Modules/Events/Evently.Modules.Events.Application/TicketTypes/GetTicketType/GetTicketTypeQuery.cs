using Evently.Common.Application.Messaging;

namespace Evently.Modules.Events.Application.TicketTypes.GetTicketType;

public sealed record GetTicketTypeQuery(string TicketTypeId) : IQuery<TicketTypeResponse>;
