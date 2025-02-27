using Evently.Common.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Customers.GetCustomer;

public sealed record GetCustomerQuery(string CustomerId) : IQuery<CustomerResponse>;
