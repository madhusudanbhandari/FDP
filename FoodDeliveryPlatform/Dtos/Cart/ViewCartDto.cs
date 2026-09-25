namespace FDP.Dtos.Cart;
public class ViewCartDto
{
    public int Id{get;set;}
    public int UserId{get;set;}
    public decimal TotalAmount{get;set;}
    public List<ViewCartItemDto> CartItems{get;set;}=new();

}