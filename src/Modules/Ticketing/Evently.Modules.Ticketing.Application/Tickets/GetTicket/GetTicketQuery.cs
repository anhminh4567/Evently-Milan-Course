using Evently.Common.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Tickets.GetTicket;

public sealed record GetTicketQuery(string TicketId) : IQuery<TicketResponse>;
