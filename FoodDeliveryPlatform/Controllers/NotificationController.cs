using FDP.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FDP.Controller;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;
    public NotificationController(INotificationService notificationService)
    {
        _notificationService=notificationService;
    }

    [HttpGet("my-notifications")]
    public async Task<IActionResult> GetMyNotifications()
    {
        var userId=int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var notification=await _notificationService.GetMyNotificationsAsync(userId);
        return Ok(notification);
    }

    [HttpPatch("{notificationId}/read")]
    public async Task<IActionResult> MarkAsRead(int notificationId)
    {
        var userId=int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        await _notificationService.MarkAsReadAsync(notificationId,userId);

        return Ok(new
        {
            message="Notification marked as read"
        });
    }


}