using FDP.Models;

namespace FDP.Interface;

public interface IPaymentProvider
{
    Task<PaymentProviderResult>ProcessPaymentAsync(
        decimal amount,
        string paymentMethod
    );
}