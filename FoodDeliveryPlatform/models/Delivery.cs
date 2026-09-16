using FDP.enums;

namespace FDP.Models;

public class Delivery
{
    public int Id{get;set;}
    public int OrderId{get;set;}
    public Order Order{get;set;}=null!;
    public int? DeliveryPersonId{get;set;}
    public User? User{get;set;}
    public DeliveryStatus DeliveryStatus{get;set;}
    public DateTime? AssignedAt{get;set;}
    public DateTime? PickedUpAt{get;set;}
    public DateTime? DeliveredAt{get;set;}

}