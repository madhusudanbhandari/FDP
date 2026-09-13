using AutoMapper;
using FDP.Dtos.Cart;
using FDP.Exceptions;
using FDP.Interface;
using FDP.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FDP.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    public CartService(ICartRepository cartRepository,IMapper mapper)
    {
        _cartRepository=cartRepository;
        _mapper=mapper;
    }

    public async Task<ViewCartDto> AddToCartAsync(int userId,AddToCartDto dto)
    {
        var cart=await _cartRepository.GetCartByUserIdAsync(userId);

        if (cart == null)
        {
            cart=new Cart
            {
                UserId=userId
            };
             _cartRepository.AddCart(cart);
            await _cartRepository.SaveChangesAsync();
        }

        var existingCartItem=await _cartRepository.GetCartItemAsync(cart.Id,dto.MenuItemId);

        if (existingCartItem != null)
        {
            existingCartItem.Quantity+=dto.Quantity;
        }
        else
        {
           var cartItem=new CartItem
            {
                CartId=cart.Id,
                MenuItemId=dto.MenuItemId,
                Quantity=dto.Quantity
            };  

             _cartRepository.AddToCart(cartItem);
        } 
       
        await _cartRepository.SaveChangesAsync();

        return  _mapper.Map<ViewCartDto>(cart);
    }

    public async Task<ViewCartItemDto> UpdateCartAsync(
        int userId,
        int cartItemId,
        UpdateCartItemDto dto
    )
    {
        var cartItem=await _cartRepository.GetCartItemAsync(cartItemId,userId);

        if (cartItem == null)
        {
            throw new NotFoundException("cart item not found");
        }

        if (dto.Quantity <= 0)
        {
            throw new BadRequestException("Quantity must be greater then 0");
        }

        cartItem.Quantity=dto.Quantity;

        await _cartRepository.SaveChangesAsync();
        var cart=await _cartRepository.GetCartByUserIdAsync(userId);

        return _mapper.Map<ViewCartItemDto> (cartItem);
    }

    public async Task<string?> RemoveCartItemAsync(int userId,  int cartItemId)
    {
        var cartItem=await _cartRepository.GetCartItemAsync(cartItemId,userId);

        if (cartItem == null)
        {
            throw new InvalidOperationException("Cannot find the cart item");

        }

        _cartRepository.RemoveCartItem(cartItem);

        await _cartRepository.SaveChangesAsync();

        var cart=await _cartRepository.GetCartByUserIdAsync(userId);

        return "Item removed successfully";

    }

    public async Task<ViewCartDto> ViewMyCartAsync(int userId)
    {
        var cart=await _cartRepository.GetCartByUserIdAsync(userId);

        return  _mapper.Map<ViewCartDto>(cart);
    }
}