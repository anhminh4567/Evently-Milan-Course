using Evently.Common.Application.Messaging;
using Evently.Modules.Ticketing.Application.Tickets.GetTicket;

namespace Evently.Modules.Ticketing.Application.Tickets.GetTicketForOrder;

public sealed record GetTicketsForOrderQuery(string OrderId) : IQuery<IReadOnlyCollection<TicketResponse>>;
