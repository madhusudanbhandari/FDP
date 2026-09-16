using FDP.enums;

namespace FDP.Dtos.Payment;

public class CreatePaymentDto
{
    public int OrderId{get;set;}
    public PaymentMethod PaymentMethod{get;set;}
}