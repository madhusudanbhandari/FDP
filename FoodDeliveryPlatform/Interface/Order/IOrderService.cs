using FDP.Dtos.Orders;

namespace FDP.Interface;

public interface IOrderService
{
    Task<ViewOrderDto> CreateOrderAsync(int userId);
    Task<ViewOrderDto?>GetOrderByIdAsync(int orderId,int userId);
    Task<List<ViewOrderDto>> GetMyOrdersAsync(int userId);
}