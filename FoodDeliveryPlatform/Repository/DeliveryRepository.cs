using FDP.Data;
using FDP.Interface;
using FDP.Models;
using Microsoft.EntityFrameworkCore;

namespace FDP.Repository;

public class DeliveryRepository : IDeliveryRepository
{
    private readonly AppDbContext _context;
    public DeliveryRepository(AppDbContext context)
    {
        _context=context;
    }

    public async Task<Delivery?> GetByIdAsync(int id)
    {
        return await _context.Deliveries.
                        Include(d=>d.Order).
                        FirstOrDefaultAsync(d=>d.Id==id);
    }

    public async Task<Delivery?> GetByOrderIdAsync(int orderId)
    {
        return await _context.Deliveries
                        .Include(d=>d.Order)
                        .FirstOrDefaultAsync(d=>d.Order.Id==orderId);

    }

    public async Task <List<Delivery>> GetAllAsync()
    {
        return await _context.Deliveries
                        .Include(d=>d.Order)
                        .AsNoTracking()
                        .ToListAsync();
    }

    public async Task<List<Delivery>> GetByDeliveryPersonIdAsync(int deliveryPersonId)
    {
        return await _context.Deliveries.
                Where(d=>d.DeliveryPersonId==deliveryPersonId)
                .Include(d=>d.Order)
                .AsNoTracking()
                .ToListAsync();
    }

    public async Task<List<Delivery>> GetAvailableDeliveriesAsync()
    {
        return await _context.Deliveries
                .Where(d=>d.DeliveryStatus==enums.DeliveryStatus.Pending)
                .Include(d=>d.Order)
                .AsNoTracking()
                .OrderBy(d=>d.Id)
                .ToListAsync();
    }

    public async Task AddAsync(Delivery delivery)
    {
        await _context.Deliveries.AddAsync(delivery);
    }

    public async Task SaveChangesAsync()
    {
         await _context.SaveChangesAsync();
    }
}