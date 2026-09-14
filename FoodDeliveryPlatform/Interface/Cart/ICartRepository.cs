using FDP.Dtos;
using FDP.Models;

namespace FDP.Interface;

public interface ICartRepository
{
    public Task<Cart?> GetCartByUserIdAsync(int userId);
    Task<CartItem?> GetCartItemByCartAndMenuItemAsync(
        int cartId,
        int menuItemId
    );
    public Task<CartItem?> GetCartItemAsync(int userId,int MenuItemId);

    public void AddCart(Cart cart);
    public void AddToCart(CartItem cartItem);
    public void RemoveCartItem(CartItem cartItem);

    Task DeleteCartAsync(Cart cart);
    public Task SaveChangesAsync();
}