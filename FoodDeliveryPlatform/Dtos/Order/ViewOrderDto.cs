namespace FDP.Dtos.Orders;

public class ViewOrderDto
{
    public int Id{get;set;}
    public int UserId{get;set;}

    public int RestaurantId{get;set;}
    public string RestaurantName{get;set;}=string.Empty;
    public decimal TotalAmount{get;set;}
    public string Status{get;set;}=string.Empty;
    public DateTime CreatedAt{get;set;}
    public List<ViewOrderItemDto>Items{get;set;}=new();
}