using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Domain.Customers;

public interface ICustomerRepository : IBaseRepository<Customer>    
{
    Task<Customer?> GetAsync(string id, CancellationToken cancellationToken = default);

    void Insert(Customer customer);
}
