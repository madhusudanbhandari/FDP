using FDP.Models;

namespace FDP.Interface;

public interface IPaymentRepository
{
    public Task<Payment?> GetPaymentByIdAsync(int id);
    public Task<Payment?> GetPaymentByOrderIdAsync(int orderId);    
    public  Task AddPayment(Payment payment);
    public Task SaveChangesAsync();
}