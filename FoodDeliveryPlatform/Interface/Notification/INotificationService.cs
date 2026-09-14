using FDP.Dtos.Notification;

namespace FDP.Interface;

public interface INotificationService
{
    public Task<List<ViewNotificationDto>> GetMyNotificationsAsync(int userId);
    Task MarkAsReadAsync(int notificationId, int userId);

    Task CreateNotificationAsync(
        int userId,
        string message
    );

}