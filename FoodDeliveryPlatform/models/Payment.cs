using FDP.enums;

namespace FDP.Models;

public class Payment
{
    public int Id{get;set;}
    public int OrderId{get;set;}
    public Order Order{get;set;}=null!;
    public string? TransactionId{get;set;}
    public decimal Amount{get;set;}
    public DateTime CreatedAt{get;set;}
    public DateTime? PaidAt{get;set;}
    public PaymentMethod PaymentMethod{get;set;} 
    public PaymentStatus PaymentStatus{get;set;}
}