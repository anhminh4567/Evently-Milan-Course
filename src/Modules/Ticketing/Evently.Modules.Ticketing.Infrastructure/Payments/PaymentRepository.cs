using Evently.Common.Infrastructure.Repositories;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Payments;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Ticketing.Infrastructure.Payments;

internal sealed class PaymentRepository : BaseRepository<Payment> ,IPaymentRepository
{
    private TicketingDbContext _context => _dbContext as TicketingDbContext;
    public PaymentRepository(TicketingDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Payment?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Payments.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Payment>> GetForEventAsync(
        Event @event,
        CancellationToken cancellationToken = default)
    {
        return await (
            from order in _context.Orders
            join payment in _context.Payments on order.Id equals payment.OrderId
            join orderItem in _context.OrderItems on order.Id equals orderItem.OrderId
            join ticketType in _context.TicketTypes on orderItem.TicketTypeId equals ticketType.Id
            where ticketType.EventId == @event.Id
            select payment).ToListAsync(cancellationToken);
    }

    public void Insert(Payment payment)
    {
        _context.Payments.Add(payment);
    }
}
