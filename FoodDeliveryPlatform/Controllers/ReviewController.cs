using Microsoft.AspNetCore.Authorization;
using FDP.Services;
using Microsoft.AspNetCore.Mvc;
using FDP.Dtos.Reviews;
using System.Security.Claims;
using System.Net.NetworkInformation;

namespace FDP.Controller;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly ReviewService _reviewService;
    public ReviewController(ReviewService reviewService)
    {
        _reviewService=reviewService;
    }

    [HttpPost("order/{orderId}")]
    public async Task<IActionResult> CreateReview(int orderId,CreateReviewDto dto)
    {
        var userId=int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var review=await _reviewService.CreateReviewAsync(userId,orderId,dto);

        return Ok(review);
    }

    [AllowAnonymous]
    [HttpGet("restaurant/{restaurantId}")]
    public async Task<IActionResult> GetRestaurantReviews(
        int restaurantId
    )
    {
        var review=await _reviewService
                    .GetRestaurantReviewAsync(restaurantId);
                
                return Ok(review);
    }




}