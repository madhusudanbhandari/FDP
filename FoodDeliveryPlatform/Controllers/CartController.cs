using System.Security.Claims;
using FDP.Dtos;
using FDP.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FDP.Controller;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    public CartController(ICartService cartService)
    {
        _cartService=cartService;
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddToCart(AddToCartDto dto)
    {
        var claim=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(!int.TryParse(claim, out int Id))
        {
            return Unauthorized();
        }

        var cart=await _cartService.AddToCartAsync(Id,dto);

        return Ok(cart);
    }

    [HttpPatch("items/{cartItemId}")]
    public async Task <IActionResult> UpdateCart(int cartItemId, UpdateCartItemDto dto)
    {
        var claim=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(!int.TryParse(claim, out int Id))
        {
            return Unauthorized();
        }

        var updated=await _cartService.UpdateCartAsync(Id,cartItemId,dto);
        return Ok(updated);
    }

    [HttpGet]
    public async Task<IActionResult> ViewCart()
    {
        var claim=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(!int.TryParse(claim, out int Id))
        {
            return Unauthorized();
        }
        var cart=await _cartService.ViewMyCartAsync(Id);
        return Ok(cart);
    }

    [HttpDelete("items/{cartItemId}")]
    public async Task<IActionResult> RemoveItem(int cartItemId)
    {
        var claim=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(!int.TryParse(claim, out int Id))
        {
            return Unauthorized();
        }

        var removed=await _cartService.RemoveCartItemAsync(Id,cartItemId);
        return Ok(removed);
    }
}