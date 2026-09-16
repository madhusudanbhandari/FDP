using FDP.enums;

namespace FDP.Dtos.Payment;

public class ViewPaymentDto
{
    private int Id{get;set;}
    public int OrderId{get;set;}
    public decimal Amount{get;set;}
    public PaymentMethod PaymentMethod{get;set;}
    public PaymentStatus PaymentStatus{get;set;}
    public string? TransactionId{get;set;}
    public DateTime CreatedAt{get;set;}
    public DateTime? PaidAt{get;set;}
}