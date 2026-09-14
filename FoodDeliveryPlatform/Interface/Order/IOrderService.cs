using FDP.Dtos.Orders;

namespace FDP.Interface;

public interface IOrderService
{
    Task<ViewOrderDto> CreateOrderAsync(int userId);
    Task<ViewOrderDto?>GetOrderByIdAsync(int orderId,int userId);
    Task<List<ViewOrderDto>> GetMyOrdersAsync(int userId);
    Task<List<ViewOrderDto>> GetOrdersOfMyRestaurant(int restaurantId);
    Task<ViewOrderDto?> UpdateOrderStatus(int userId,int orderId,UpdateOrderStatusDto dto);
    Task  CancelOrderAsync(int orderId, int userId);
}