using System.Security.Claims;
using FDP.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FDP.Controller;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService=orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder()
    {
       var userId=int.Parse(
        User.FindFirstValue(ClaimTypes.NameIdentifier)!
       );

    var order=await _orderService.CreateOrderAsync(userId);

    return CreatedAtAction(
        nameof(GetOrderById),
        new{id=order.Id},
        order
    );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var userId=int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var order=await _orderService.GetOrderByIdAsync(id,userId);

        if(order==null)
            return NotFound();

        return Ok(order);
    }

    [HttpGet("my-orders")]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId=int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var orders=await _orderService.GetMyOrdersAsync(userId);

        return Ok(orders);
    }

}