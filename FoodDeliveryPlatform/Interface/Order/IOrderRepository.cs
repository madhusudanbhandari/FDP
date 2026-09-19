
using FDP.Models;

namespace FDP.Interface;

public interface IOrderRepository
{
    Task<Order> CreateOrderAsync(Order order);
    Task<Order?> GetOrderByIdAsync(int orderId);
    Task<List<Order>> GetOrderByUserIdAsync(int userId);
    Task<List<Order>>GetOrdersByRestaurantId(int restaurantId);
    Task<Order?> GetOrderForOwnerAsync(int orderId, int ownerId);
    Task<Order?> GetOrderForCustomerAsync(int oderId, int userId);
    Task SaveChangesAsync();

    Task <List<Order>> GetExpiredPendingOrdersAsync();
}