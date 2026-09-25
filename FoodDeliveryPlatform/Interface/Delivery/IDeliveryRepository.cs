using FDP.Models;

namespace FDP.Interface;

public interface IDeliveryRepository
{
    Task<Delivery?> GetByIdAsync(int id);
    Task<Delivery?>GetByOrderIdAsync(int orderId);
    Task<List<Delivery>> GetAllAsync();
    Task<List<Delivery>> GetByDeliveryPersonIdAsync(int deliveryPersonId);
    Task<List<Delivery>> GetAvailableDeliveriesAsync();
    Task AddAsync(Delivery delivery);
    Task SaveChangesAsync();
}