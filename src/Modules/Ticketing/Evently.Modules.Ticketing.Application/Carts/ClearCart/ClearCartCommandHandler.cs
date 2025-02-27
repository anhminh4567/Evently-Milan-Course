using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Ticketing.Domain.Customers;

namespace Evently.Modules.Ticketing.Application.Carts.ClearCart;

internal sealed class ClearCartCommandHandler  : ICommandHandler<ClearCartCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly CartService _cartService;

    public ClearCartCommandHandler(ICustomerRepository customerRepository, CartService cartService)
    {
        _customerRepository = customerRepository;
        _cartService = cartService;
    }

    public async Task<Result> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        Customer? customer = await _customerRepository.GetAsync(request.CustomerId, cancellationToken);

        if (customer is null)
        {
            return Result.Failure(CustomerErrors.NotFound(request.CustomerId));
        }

        await _cartService.ClearAsync(customer.Id, cancellationToken);

        return Result.Success();
    }
}
