using System.Security.Claims;
using FDP.Dtos.Delivery;
using FDP.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FDP.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DeliveryController : ControllerBase
{
    private readonly IDeliveryService _deliveryService;

    public DeliveryController(IDeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }

    // ADMIN
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var deliveries = await _deliveryService.GetAllAsync();

        return Ok(deliveries);
    }

    // ADMIN
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var delivery = await _deliveryService.GetByIdAsync(id);

        if (delivery == null)
            return NotFound();

        return Ok(delivery);
    }

    // ADMIN
    [HttpPost("{id}/assign")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Assign(
        int id,
        AssignDeliveryDto dto)
    {
        await _deliveryService.AssignDeliveryAsync(
            id,
            dto.DeliveryPersonId);

        return Ok(new
        {
            message = "Delivery assigned successfully."
        });
    }

    // DELIVERY PERSON
    [HttpGet("my")]
    [Authorize(Roles = "DeliveryPerson")]
    public async Task<IActionResult> GetMyDeliveries()
    {
        var userId = GetCurrentUserId();

        var deliveries = await _deliveryService
            .GetMyDeliveriesAsync(userId);

        return Ok(deliveries);
    }

    // DELIVERY PERSON
    [HttpPatch("{id}/pickup")]
    [Authorize(Roles = "DeliveryPerson")]
    public async Task<IActionResult> Pickup(int id)
    {
        var userId = GetCurrentUserId();

        await _deliveryService.MarkPickedUpAsync(
            id,
            userId);

        return Ok(new
        {
            message = "Delivery marked as picked up."
        });
    }

    // DELIVERY PERSON
    [HttpPatch("{id}/out-for-delivery")]
    [Authorize(Roles = "DeliveryPerson")]
    public async Task<IActionResult> OutForDelivery(int id)
    {
        var userId = GetCurrentUserId();

        await _deliveryService.MarkOutForDeliveryAsync(
            id,
            userId);

        return Ok(new
        {
            message = "Delivery is now out for delivery."
        });
    }

    // DELIVERY PERSON
    [HttpPatch("{id}/delivered")]
    [Authorize(Roles = "DeliveryPerson")]
    public async Task<IActionResult> Delivered(int id)
    {
        var userId = GetCurrentUserId();

        await _deliveryService.MarkDeliveredAsync(
            id,
            userId);

        return Ok(new
        {
            message = "Delivery marked as delivered."
        });
    }

    // CUSTOMER
    [HttpGet("order/{orderId}")]
    [Authorize]
    public async Task<IActionResult> GetByOrder(
        int orderId)
    {
        var userId = GetCurrentUserId();

        var delivery = await _deliveryService
            .GetByOrderIdAsync(orderId, userId);

        if (delivery == null)
            return NotFound();

        return Ok(delivery);
    }

    private int GetCurrentUserId()
    {
        var claim = User.FindFirst(
            ClaimTypes.NameIdentifier);

        if (claim == null)
            throw new UnauthorizedAccessException();

        return int.Parse(claim.Value);
    }
}