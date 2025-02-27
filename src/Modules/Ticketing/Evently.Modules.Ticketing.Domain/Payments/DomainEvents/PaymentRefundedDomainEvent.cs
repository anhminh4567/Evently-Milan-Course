using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Domain.Payments;

public sealed record PaymentRefundedDomainEvent(string paymentId, string transactionId, decimal refundAmount)
    : DomainEvent
{
    public string PaymentId { get; init; } = paymentId;

    public string TransactionId { get; init; } = transactionId;

    public decimal RefundAmount { get; init; } = refundAmount;
}
