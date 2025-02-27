using Evently.Common.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Tickets.CreateTicketBatch;

public sealed record CreateTicketBatchCommand(string OrderId) : ICommand;
