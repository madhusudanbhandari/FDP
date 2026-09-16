using FDP.Interface;
using FDP.Models;

namespace FDP.Services;

public class MockPaymentProvider : IPaymentProvider
{
    public Task<PaymentProviderResult>ProcessPaymentAsync(
        decimal amount,
        string paymentMethod
    )
    {
         var transactionId=$"MOCK-{Guid.NewGuid():N}";

         return Task.FromResult(
            new PaymentProviderResult
            {
                Sucess=true,
                TransactionId=transactionId
            }
         );
    }
}