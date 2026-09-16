using FDP.enums;

namespace FDP.Dtos.Delivery;

public class ViewDeliveryDto
{
    public int Id{get;set;}
    public int OrderId{get;set;}
    public int? DeliveryPersonId{get;set;}
    public DeliveryStatus DeliveryStatus{get;set;}
    public DateTime? AssignedAt{get;set;}
    public DateTime? PickedUpAt{get;set;}
    public DateTime? DeliveredAt{get;set;}

}