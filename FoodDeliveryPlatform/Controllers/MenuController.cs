using FDP.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FDP.Controller;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IMenuService _menuService;
    public MenuController(IMenuService menuService)
    {
        _menuService=menuService;
    }

    [Authorize(Roles ="RestaurantOwner")]
    [HttpPost("create-menu")]
    public async Task<IActionResult> CreateMenuAsync(int restaurantId)
    {

        var claim=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (claim == null)
        {
            return Unauthorized();
        }

        if(!int.TryParse(claim,out int Id))
        {
            return Unauthorized();
        }

        var menu=await _menuService.CreateMenuAsync(restaurantId,Id);
        return Ok(menu);
    }

    [HttpGet("get-all-menus")]
    public async Task<IActionResult> GetAllMenusAsync()
    {
        var menus=await _menuService.GetAllMenusAsync();
        return Ok(menus);
    }

    [HttpGet("get-menu-byId")]
    public async Task<IActionResult> GetMenuById(int id)
    {
        var menu=await _menuService.GetMenuByIdAsync(id);
        return Ok(menu);
    }

    [HttpDelete("delete-menu")]
    public async Task<IActionResult> DeleteMenuAsync(int id)
    {
        var isDeleted=await _menuService.DeleteMenuAsync(id);
        return Ok(isDeleted);
    }


    
}