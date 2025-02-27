using Evently.Modules.Ticketing.Application.Abstractions.Payments;

namespace Evently.Modules.Ticketing.Infrastructure.Payments;

internal sealed class PaymentService : IPaymentService
{
    public Task<PaymentResponse> ChargeAsync(decimal amount, string currency)
    {
        return Task.FromResult(new PaymentResponse(Guid.NewGuid().ToString(), amount, currency));
    }

    public Task RefundAsync(string transactionId, decimal amount)
    {
        return Task.CompletedTask;
    }
}
