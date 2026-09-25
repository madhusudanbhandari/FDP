namespace FDP.Dtos.Cart;

public class ViewCartItemDto
{
    public int Id { get; set; }

    public int CartId { get; set; }

    public int MenuItemId { get; set; }

    public int Quantity { get; set; }

    public string MenuItemName { get; set; } = string.Empty;

    public decimal Price { get; set; }
}