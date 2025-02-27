using Evently.Common.Domain;
using Evently.Modules.Ticketing.Domain.Events;

namespace Evently.Modules.Ticketing.Domain.Payments;

public interface IPaymentRepository : IBaseRepository<Payment>
{
    Task<Payment?> GetAsync(string id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Payment>> GetForEventAsync(Event @event, CancellationToken cancellationToken = default);

    void Insert(Payment payment);
}
