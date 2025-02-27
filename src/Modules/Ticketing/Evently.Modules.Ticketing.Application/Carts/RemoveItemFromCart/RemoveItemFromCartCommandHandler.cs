using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Events;

namespace Evently.Modules.Ticketing.Application.Carts.RemoveItemFromCart;

internal sealed class RemoveItemFromCartCommandHandler  : ICommandHandler<RemoveItemFromCartCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ITicketTypeRepository _ticketTypeRepository;
    private readonly CartService _cartService;

    public RemoveItemFromCartCommandHandler(ICustomerRepository customerRepository, ITicketTypeRepository ticketTypeRepository, CartService cartService)
    {
        _customerRepository = customerRepository;
        _ticketTypeRepository = ticketTypeRepository;
        _cartService = cartService;
    }

    public async Task<Result> Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
    {
        Customer? customer = await _customerRepository.GetAsync(request.CustomerId, cancellationToken);

        if (customer is null)
        {
            return Result.Failure(CustomerErrors.NotFound(request.CustomerId));
        }

        TicketType? ticketType = await _ticketTypeRepository.GetAsync(request.TicketTypeId, cancellationToken);

        if (ticketType is null)
        {
            return Result.Failure(TicketTypeErrors.NotFound(request.TicketTypeId));
        }

        await _cartService.RemoveItemAsync(customer.Id, ticketType.Id, cancellationToken);

        return Result.Success();
    }
}
