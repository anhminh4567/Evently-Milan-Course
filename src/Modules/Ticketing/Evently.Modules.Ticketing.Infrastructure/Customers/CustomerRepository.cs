using Evently.Modules.Events.Infrastructure.Database;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Ticketing.Infrastructure.Customers;

internal sealed class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    private TicketingDbContext _context => _dbContext as TicketingDbContext;
    public CustomerRepository(TicketingDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Customer?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Customers.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public void Insert(Customer customer)
    {
        _context.Customers.Add(customer);
    }
}
