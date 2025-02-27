namespace Evently.Modules.Ticketing.PublicApi;

public interface ITicketingApi
{
    Task CreateCustomerAsync(
        string customerId,
        string email,
        string firstName,
        string lastName,
        CancellationToken cancellationToken = default);
}
