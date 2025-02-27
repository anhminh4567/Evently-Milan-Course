using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Domain.Payments;

public sealed record PaymentCreatedDomainEvent(string paymentId) : DomainEvent
{
    public string PaymentId { get; init; } = paymentId;
}
