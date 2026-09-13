namespace FDP.Dtos.Orders;

public class ViewOrderItemDto
{
    public int Id{get;set;}
    public int MenuItemId{get;set;}
    public string MenuItemName{get;set;}=string.Empty;
    public int Quantity{get;set;}
    public decimal UnitPrice{get;set;}
    public decimal SubTotal{get;set;}
}