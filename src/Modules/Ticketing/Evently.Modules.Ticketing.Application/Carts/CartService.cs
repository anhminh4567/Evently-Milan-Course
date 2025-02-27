using Evently.Common.Application.Caching;

namespace Evently.Modules.Ticketing.Application.Carts;

public sealed class CartService
{
    private readonly ICacheService _cacheService;
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(20);

    public CartService(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<Cart> GetAsync(string customerId, CancellationToken cancellationToken = default)
    {
        string cacheKey = CreateCacheKey(customerId);

        Cart cart = await _cacheService.GetAsync<Cart>(cacheKey, cancellationToken) ??
                    Cart.CreateDefault(customerId);

        return cart;
    }

    public async Task ClearAsync(string customerId, CancellationToken cancellationToken = default)
    {
        string cacheKey = CreateCacheKey(customerId);

        var cart = Cart.CreateDefault(customerId);

        await _cacheService.SetAsync(cacheKey, cart, DefaultExpiration, cancellationToken);
    }

    public async Task AddItemAsync(string customerId, CartItem cartItem, CancellationToken cancellationToken = default)
    {
        string cacheKey = CreateCacheKey(customerId);

        Cart cart = await GetAsync(customerId, cancellationToken);

        CartItem? existingCartItem = cart.Items.Find(c => c.TicketTypeId == cartItem.TicketTypeId);

        if (existingCartItem is null)
        {
            cart.Items.Add(cartItem);
        }
        else
        {
            existingCartItem.Quantity += cartItem.Quantity;
        }

        await _cacheService.SetAsync(cacheKey, cart, DefaultExpiration, cancellationToken);
    }

    public async Task RemoveItemAsync(string customerId, string ticketTypeId, CancellationToken cancellationToken = default)
    {
        string cacheKey = CreateCacheKey(customerId);

        Cart cart = await GetAsync(customerId, cancellationToken);

        CartItem? cartItem = cart.Items.Find(c => c.TicketTypeId == ticketTypeId);

        if (cartItem is null)
        {
            return;
        }

        cart.Items.Remove(cartItem);

        await _cacheService.SetAsync(cacheKey, cart, DefaultExpiration, cancellationToken);
    }

    private static string CreateCacheKey(string customerId) => $"carts:{customerId}";
}
