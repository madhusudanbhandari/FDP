using FDP.Dtos.Delivery;

namespace FDP.Interface;

public interface IDeliveryService
{
    Task<ViewDeliveryDto?> GetByIdAsync(int id);
    Task<ViewDeliveryDto?> GetByOrderIdAsync(int orderId, int userId);
    Task<List<ViewDeliveryDto>> GetAllAsync();
    Task<List<ViewDeliveryDto>> GetMyDeliveriesAsync(int deliveryPersonId);

    Task AssignDeliveryAsync(int deliveryId,int deliveryPersonId);
    Task MarkPickedUpAsync(int deliveryId, int deliveryPersonId);
    Task MarkOutForDeliveryAsync(int deliveryId, int deliveryPersonId);

    Task MarkDeliveredAsync(int deliveryId, int deliveryPersonId);
}