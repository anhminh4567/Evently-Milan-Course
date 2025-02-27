namespace Evently.Modules.Ticketing.Application.Carts;

public sealed class Cart
{
    public string CustomerId { get; init; }

    public List<CartItem> Items { get; init; } = [];

    internal static Cart CreateDefault(string customerId) => new() { CustomerId = customerId };
}
