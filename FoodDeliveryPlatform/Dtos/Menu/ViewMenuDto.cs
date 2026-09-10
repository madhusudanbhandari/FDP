namespace FDP.Dtos.Menu;
public class ViewMenuDto
{
    public int Id{get;set;}

    public string Category{get;set;}=string.Empty;
    public string ItemName{get;set;}=string.Empty;
    public int ItemPrice{get;set;}
    public int quantity{get;set;}

    public int RestaurantId{get;set;}
}