using AutoMapper;
using FDP.Dtos.Notification;
using FDP.Exceptions;
using FDP.Interface;
using FDP.Models;

namespace FDP.Services;

public class NotificationService:INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IMapper _mapper;

    public NotificationService(INotificationRepository notificationRepository,IMapper mapper)
    {
        _notificationRepository=notificationRepository;
        _mapper=mapper;
    }

    public async Task<List<ViewNotificationDto>> GetMyNotificationsAsync(int userId)
    {
        var notifications=await _notificationRepository.GetNotificationsByUserAsync(userId);

        return _mapper.Map<List<ViewNotificationDto>>(notifications);

    }

    public async Task MarkAsReadAsync(int notificationId, int userId)
    {
        var notification=await _notificationRepository.GetNotificationByIdAsync(notificationId,userId);

        if (notification == null)
        {
            throw new NotFoundException("Cannot find notification");
        }
        notification.IsRead=true;
        await _notificationRepository.SaveChangesAsync();
    }

    public async Task CreateNotificationAsync(int userId, string message)
    {
        var notification=new Notification
        {
            UserId=userId,
            Message=message,
            IsRead=false,
            CreatedAt=DateTime.UtcNow,
        };

         _notificationRepository.AddNotification(notification);
         await _notificationRepository.SaveChangesAsync();
    }
}