using Evently.Common.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Payments.RefundPaymentsForEvent;

public sealed record RefundPaymentsForEventCommand(string EventId) : ICommand;
