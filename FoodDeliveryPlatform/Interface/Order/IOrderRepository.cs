
using FDP.Models;

namespace FDP.Interface;

public interface IOrderRepository
{
    Task<Order> CreateOrderAsync(Order order);
    Task<Order?> GetOrderByIdAsync(int orderId);
    Task<List<Order>> GetOrderByUserIdAsync(int userId);
}