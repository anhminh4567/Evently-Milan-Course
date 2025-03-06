using Evently.Common.Infrastructure.Repositories;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Ticketing.Infrastructure.Orders;

internal sealed class OrderRepository: BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(TicketingDbContext dbContext) : base(dbContext)
    {
    }

    private TicketingDbContext _context => _dbContext as TicketingDbContext;

    public async Task<Order?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .SingleOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public void Insert(Order order)
    {
        _context.Orders.Add(order);
    }
}
