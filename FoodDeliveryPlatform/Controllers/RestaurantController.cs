
using FDP.Dtos.Restaurant;
using FDP.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
        var restaurant=await _restaurantService.CreateRestaurantAsync(dto);
        return Ok(restaurant);
    }

    [Authorize(Roles ="RestaurantOwner")]
    [HttpPatch("update-restaurant")]
    public async Task<IActionResult> UpdateRestaurant(int id,UpdateRestaurantDto dto)
    {
        var updated=await _restaurantService.UpdateRestaurantAsync(id,dto);
        return Ok(updated);
    }

    [HttpGet("view-restaurant")]
    public async Task<IActionResult> ViewRestaurant(int id)
    {
        var restaurant=await _restaurantService.SeeRestaurantAsync(id);
        return Ok(restaurant);
    }

    [Authorize(Roles ="Admin,Customer")]
    [HttpGet("View-all-restaurants")]
    public async Task<IActionResult> ViewAllRestaurants()
    {
        var restaurants=await _restaurantService.SeeAllRestaurantsAsync();
        return Ok(restaurants);
    }


}