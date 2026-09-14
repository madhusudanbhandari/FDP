using System.Security.Authentication;
using FDP.Data;
using FDP.Interface;
using FDP.Models;
using Microsoft.EntityFrameworkCore;

namespace FDP.Repository;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;
    public CartRepository(AppDbContext context)
    {
        _context=context;
    }

    public async Task<Cart?> GetCartByUserIdAsync(int userId)
    {
       return await _context.Carts
                .Include(c=>c.CartItems)
                    .ThenInclude(ci=>ci.MenuItem)
                        .ThenInclude(mi=>mi.Menu)
                .FirstOrDefaultAsync(c=>c.UserId==userId); 
    }
       
    public async Task<CartItem?> GetCartItemAsync(int cartItemId,int userId)
    {
        return await _context.CartItems
                .Include(ci=>ci.Cart)
                .FirstOrDefaultAsync(ci=>
                ci.Id==cartItemId &&
                ci.Cart.UserId==userId);
    }

    public async Task<CartItem?> GetCartItemByCartAndMenuItemAsync(int cartId,int menuItemId)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(
                ci=>ci.CartId==cartId &&
                ci.MenuItemId==menuItemId
            );
    }
    public void AddCart(Cart cart)
    {
        _context.Carts.Add(cart);
    }
    public void AddToCart(CartItem cartItem)
    {
         _context.CartItems.Add(cartItem);
    }


    public void  RemoveCartItem(CartItem cartItem)
    {
        _context.CartItems.Remove(cartItem);
    }

    public async Task DeleteCartAsync(Cart cart)
    {
        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}