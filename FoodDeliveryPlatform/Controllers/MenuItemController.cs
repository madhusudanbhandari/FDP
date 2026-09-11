using FDP.Services;
using Microsoft.AspNetCore.Mvc;
using FDP.Interface;
using FDP.Dtos.MenuItem;
using System.IO.Pipelines;
using System.Security.Claims;

namespace FDP.Controller;

[ApiController]
[Route("api/[controller]")]
public class MenuItemController : ControllerBase
{
    private readonly IMenuItemService _menuItemService;
    public MenuItemController(IMenuItemService menuItemService)
    {
        _menuItemService=menuItemService;
    }

    [HttpPost("create-menuItem")]
    public async Task<IActionResult>CreateMenuItemAsync(CreateMenuItemDto dto)
    {
        var claim=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(!int.TryParse(claim,out int id))
        {
            return Unauthorized();
        }

        var menuItem=await _menuItemService.CreateMenuItemAsync(dto,id);
        return Ok(menuItem);
    }

    [HttpPatch("update-menuItem")]
    public async Task<IActionResult> UpdateMenuItem(int id, UpdateMenuItemDto dto)
    {
        var claim=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(!int.TryParse(claim,out int Id))
        {
            return Unauthorized();
        }

        var item=await _menuItemService.UpdateMenuItemAsync(id,dto,Id);
        return Ok(item);
    }

    [HttpGet("see-menuItem")]
    public async Task<IActionResult> SeeMenuItem(int id)
    {
        var item=await _menuItemService.SeeMenuItemByIdAsync(id);
        return Ok(item);
    }

    [HttpGet("see-all-menuItems")]
    public async Task<IActionResult> seeAllMenuItemsAsync()
    {
        var items=await _menuItemService.SeeAllMenuItems();
        return Ok(items);
    }

    [HttpDelete("delete-menuItem")]
    public async Task<IActionResult> DeleteMenuItem(int id)
    {
         var claim=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(!int.TryParse(claim,out int Id))
        {
            return Unauthorized();
        }

        var deletedItem=await _menuItemService.DeleteMenuItem(id,Id);
        return Ok(deletedItem);
    }
}