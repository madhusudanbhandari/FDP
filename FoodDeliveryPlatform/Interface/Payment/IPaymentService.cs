using FDP.Dtos.Payment;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FDP.Interface;

public interface IPaymentService
{
    public Task<ViewPaymentDto> CreatePaymentAsync(int userId, CreatePaymentDto dto);
    public Task<ViewPaymentDto?> GetPaymentByIdAsync(int userId,int paymentId);
    public Task<ViewPaymentDto?> GetPaymentByOrderIdAsync(int userId,int orderId);
}