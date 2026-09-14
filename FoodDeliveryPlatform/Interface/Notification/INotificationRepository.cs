using FDP.Models;

namespace FDP.Interface;

public interface INotificationRepository
{
    Task<List<Notification>> GetNotificationsByUserAsync(int userId);
    Task<Notification?>GetNotificationByIdAsync(
        int notificationId,
        int userId
    ) ;

    void AddNotification(Notification notification);
    Task SaveChangesAsync();
}