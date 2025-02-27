using Evently.Common.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Payments.RefundPayment;

public sealed record RefundPaymentCommand(string PaymentId, decimal Amount) : ICommand;
