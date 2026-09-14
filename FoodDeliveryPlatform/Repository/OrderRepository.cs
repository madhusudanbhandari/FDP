using FDP.Data;
using FDP.Interface;
using FDP.Models;
using Microsoft.EntityFrameworkCore;

namespace FDP.Repository;

public class OrderRepository: IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context=context;
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        return await _context.Orders
            .Include(o=>o.OrderItems)
                .ThenInclude(oi=>oi.MenuItem)
            .FirstOrDefaultAsync(o=>o.Id==orderId);
    }

    public async Task<List<Order>> GetOrderByUserIdAsync(int userId)
    {
        return await _context.Orders 
                .Where(o=>o.UserId==userId)
                .Include(o=>o.Restaurant)
                .Include(o=>o.OrderItems)
                    .ThenInclude(oi=>oi.MenuItem)
                .OrderByDescending(o=>o.CreatedAt)
                .ToListAsync();
    }

    public async Task<List<Order>> GetOrdersByRestaurantId(int restaurantId)
    {
        return await _context.Orders
            .Where(r=>r.RestaurantId==restaurantId)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderForOwnerAsync(int orderId, int ownerId)
    {
        return await _context.Orders
            .Include(o=>o.Restaurant)
            .FirstOrDefaultAsync(o=>
            o.Id==orderId &&
            o.Restaurant.OwnerId==ownerId);
    }

    public async Task<Order?> GetOrderForCustomerAsync(int orderId, int userId)
    {
        return await _context.Orders
            .Include(o=>o.OrderItems)
                .ThenInclude(oi=>oi.MenuItem)
            .FirstOrDefaultAsync(o=>
            o.Id==orderId &&
            o.UserId==userId);
    }

    public Task SaveChangesAsync()
    {
      return _context.SaveChangesAsync();
    }
}