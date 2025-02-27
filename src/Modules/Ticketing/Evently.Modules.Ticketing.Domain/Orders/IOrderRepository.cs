using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Domain.Orders;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<Order?> GetAsync(string id, CancellationToken cancellationToken = default);

    void Insert(Order order);
}
