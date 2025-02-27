using Evently.Common.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Customers.UpdateCustomer;

public sealed record UpdateCustomerCommand(string CustomerId, string FirstName, string LastName) : ICommand;
