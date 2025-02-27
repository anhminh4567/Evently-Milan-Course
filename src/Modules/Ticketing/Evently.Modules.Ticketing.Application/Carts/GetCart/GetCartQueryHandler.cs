using Evently.Common.Application.Messaging;
using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Application.Carts.GetCart;

internal sealed class GetCartQueryHandler : IQueryHandler<GetCartQuery, Cart>
{
    private readonly CartService _cartService;

    public GetCartQueryHandler(CartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<Result<Cart>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        return await _cartService.GetAsync(request.CustomerId, cancellationToken);
    }
}
