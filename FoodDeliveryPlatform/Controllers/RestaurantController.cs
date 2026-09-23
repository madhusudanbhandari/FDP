
using FDP.Dtos.Restaurant;
using FDP.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using StackExchange.Redis;

namespace FDP.Controller;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RestaurantController : ControllerBase
{
    private readonly IRestaurantService _restaurantService;

    public RestaurantController(IRestaurantService restaurantService)
    {
        _restaurantService=restaurantService;
    }

    [Authorize(Roles ="RestaurantOwner")]
    [HttpPost("create-restaurant")]
    public async Task<IActionResult> CreateRestaurant(CreateRestaurantDto dto)
    {
        var claim=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int.TryParse(claim, out int Id);

        var restaurant=await _restaurantService.CreateRestaurantAsync(dto,Id);
        return Ok(restaurant);
    }

    [Authorize(Roles ="RestaurantOwner")]
    [HttpPatch("update-restaurant")]
    public async Task<IActionResult> UpdateRestaurant(int id,UpdateRestaurantDto dto)
    {
        var claim=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int.TryParse(claim, out var Id);

        var updated=await _restaurantService.UpdateRestaurantAsync(id,dto,Id);
        return Ok(updated);
    }

    [HttpGet("view-restaurant")]
    public async Task<IActionResult> ViewRestaurant(int id)
    {
        var restaurant=await _restaurantService.SeeRestaurantAsync(id);
        return Ok(restaurant);
    }

    [HttpGet("View-all-restaurants")]
    public async Task<IActionResult> ViewAllRestaurants([FromQuery]RestaurantQueryDto query)
    {
        var restaurants=await _restaurantService.SeeAllRestaurantsAsync(query);
        return Ok(restaurants);
    }

    [Authorize(Roles ="Admin,RestaurantOwner")]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteRestaurant(int id)
    {
        var claim=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int.TryParse(claim,out int Id);

        var restaurant=await _restaurantService.DeleteRestaurantAsync(id,Id);

        return Ok(restaurant);
    }

    [Authorize(Roles="RestaurantOwner")]
    [HttpGet("my-restaurants")]
    public async Task<IActionResult> GetMyRestaurant()
    {
        var claim=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(!int.TryParse(claim,out int ownerId))
        {
            return Unauthorized();
        }

        var restaurant=await _restaurantService.GetMyRestaurantAsync(ownerId);

        if (restaurant == null)
        {
            return NotFound();
        }

        return Ok(restaurant);
    }


}