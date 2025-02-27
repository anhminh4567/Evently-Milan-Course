namespace Evently.Modules.Ticketing.Application.Abstractions.Payments;

public interface IPaymentService
{
    Task<PaymentResponse> ChargeAsync(decimal amount, string currency);

    Task RefundAsync(string transactionId, decimal amount);
}
