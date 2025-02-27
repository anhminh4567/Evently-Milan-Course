namespace Evently.Modules.Ticketing.Application.Abstractions.Payments;

public sealed record PaymentResponse(string TransactionId, decimal Amount, string Currency);
