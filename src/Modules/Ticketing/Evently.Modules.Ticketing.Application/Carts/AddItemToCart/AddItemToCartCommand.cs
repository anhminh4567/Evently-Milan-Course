using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Events.PublicApi;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Events;

namespace Evently.Modules.Ticketing.Application.Carts.AddItemToCart;

public sealed record AddItemToCartCommand(string CustomerId, string TicketTypeId, decimal Quantity) : ICommand;

internal sealed class AddItemToCartCommandHandler
    : ICommandHandler<AddItemToCartCommand>
{
    private readonly CartService _cartService;
    private readonly ICustomerRepository _customerRepository;
    private readonly IEventsApi _eventsApi;

    public AddItemToCartCommandHandler(CartService cartService, ICustomerRepository customerRepository, IEventsApi eventsApi)
    {
        _cartService = cartService;
        _customerRepository = customerRepository;
        _eventsApi = eventsApi;
    }

    public async Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        // 1. Get customer
        // this useed to use IUserApi
        // but since we duplicate data when user is registered in User.Module
        // we dont have to call it again, just get from repository from local db
        Customer? customer = await _customerRepository.GetAsync(request.CustomerId, cancellationToken);
        if (customer is null)
            return Result.Failure(CustomerErrors.NotFound(request.CustomerId));

        // 2. Get ticket type
        TicketTypeResponse? ticketType = await _eventsApi.GetTicketTypeAsync(request.TicketTypeId, cancellationToken);
        if (ticketType is null)
            return Result.Failure(TicketTypeErrors.NotFound(request.TicketTypeId));

        // 3. Add item to cart
        var cartItem = new CartItem
        {
            TicketTypeId = ticketType.Id,
            Price = ticketType.Price,
            Quantity = request.Quantity,
            Currency = ticketType.Currency
        };
        await _cartService.AddItemAsync(customer.Id, cartItem, cancellationToken);

        return Result.Success();
    }
}
