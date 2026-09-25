using FDP.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FDP.Dtos.Payment;
using System.Security.Claims;

namespace FDP.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService=paymentService;
    }

    [HttpPost("order/{orderId}")]
    public async Task<IActionResult> CreatePayment(int orderId,CreatePaymentDto dto)
    {
        var userId=int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        dto.OrderId=orderId;

        var payment=await _paymentService.CreatePaymentAsync(userId,dto);
        return Ok(payment);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPayment(int id)
    {
        var userId=int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var payment=await _paymentService.GetPaymentByIdAsync(id,userId);
        return Ok(payment);
    }

    [HttpGet("Order/{OrderId}")]
    public async Task<IActionResult> GetPaymentByOrderId(int OrderId)
    {   
        var userId=int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var payments=await _paymentService.GetPaymentByOrderIdAsync(userId,OrderId);
        return Ok(payments);
    }

}