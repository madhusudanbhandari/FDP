using FDP.Data;
using FDP.Interface;
using FDP.Models;
using FoodDeliveryPlatform.Migrations;
using Microsoft.EntityFrameworkCore;

namespace FDP.Repository;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _context;

    public NotificationRepository(AppDbContext context)
    {
        _context=context;
    }

    public async Task<List<Notification>> GetNotificationsByUserAsync(int userId)
    {
        return await _context.Notifications
                .Where(n=>n.UserId==userId)
                .OrderByDescending(n=>n.CreatedAt)
                .ToListAsync();

    }

    public async Task<Notification?> GetNotificationByIdAsync(int id,int userId)
    {
        return await _context.Notifications
                .Where(n=>n.Id==id &&
                        n.UserId==userId)
                .FirstOrDefaultAsync();
    }

    public  void AddNotification(Notification notification)
    {
         _context.Notifications.Add(notification);
    }

    public Task  SaveChangesAsync()
    {
       return _context.SaveChangesAsync();
    }
}