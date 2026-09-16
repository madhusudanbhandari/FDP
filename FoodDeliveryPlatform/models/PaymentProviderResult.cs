namespace FDP.Models;

public class PaymentProviderResult
{
    public bool Sucess{get;set;}
    public string? TransactionId{get;set;}
    public string? ErrorMessage{get;set;}
}