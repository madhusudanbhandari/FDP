using FDP.Data;
using FDP.Interface;
using FDP.Models;
using Microsoft.EntityFrameworkCore;

namespace FDP.Repository;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context=context;
    }


    public async Task<Payment?> GetPaymentByIdAsync(int id)
    {
        return await _context.Payments
                        .Include(p=>p.Order)
                        .FirstOrDefaultAsync(p=>p.Id==id);
    }

  
    public async Task<Payment?> GetPaymentByOrderIdAsync(int orderId)
    {
        return await _context.Payments.FirstOrDefaultAsync(p=>p.OrderId==orderId);
    }

    public async Task AddPayment(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}