using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Events.PublicApi;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Users.PublicApi;

namespace Evently.Modules.Ticketing.Application.Carts.AddItemToCart;

public sealed record AddItemToCartCommand(string CustomerId, string TicketTypeId, decimal Quantity) : ICommand;

internal sealed class AddItemToCartCommandHandler
    : ICommandHandler<AddItemToCartCommand>
{
    private readonly CartService _cartService;
    private readonly IUserApi _userApi;
    private readonly IEventsApi _eventsApi;

    public AddItemToCartCommandHandler(CartService cartService, IUserApi userApi, IEventsApi eventsApi)
    {
        _cartService = cartService;
        _userApi = userApi;
        _eventsApi = eventsApi;
    }

    public async Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        // 1. Get customer
        UserResponse? customer = await _userApi.GetAsync(request.CustomerId, cancellationToken);
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
