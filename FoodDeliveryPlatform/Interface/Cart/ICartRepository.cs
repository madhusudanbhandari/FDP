using FDP.Dtos;
using FDP.Models;

namespace FDP.Interface;

public interface ICartRepository
{
    public Task<Cart?> GetCartByUserIdAsync(int userId);
    public Task<CartItem?> GetCartItemAsync(int userId,int MenuItemId);

    public void AddCart(Cart cart);
    public void AddToCart(CartItem cartItem);
    public void RemoveCartItem(CartItem cartItem);
    public Task SaveChangesAsync();
}