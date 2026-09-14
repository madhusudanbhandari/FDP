using System.Security.Claims;
using FDP.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FDP.Dtos.Orders;
using FDP.Models;

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

    [Authorize(Roles ="RestaurantOwner")]
    [HttpGet("restaurant-orders")]
    public async Task<IActionResult> GetOrdersByRestaurant()
    {
        var userId=int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var order=await _orderService.GetOrdersOfMyRestaurant(userId);
        return Ok(order);
    }

    [Authorize(Roles ="RestaurantOwner")]
    [HttpPatch("update-order-status")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, UpdateOrderStatusDto dto)
    {
        var userId=int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var updated=await _orderService.UpdateOrderStatus(userId,orderId,dto);
        return Ok(updated);
    }

    [HttpPatch("{orderId}/cancel")]
    public async Task<IActionResult> CancelOrder(int orderId)
    {
        var userId=int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        await _orderService.CancelOrderAsync(orderId,userId);

        return Ok(new
        {
            message="Order cancelled successfully"
        });
    }

}