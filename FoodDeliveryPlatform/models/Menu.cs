using FDP.Models;

public class Menu
{
    public int Id{get;set;}

    public int RestaurantId{get;set;}
    public Restaurant? Restaurant{get;set;}
    public ICollection<MenuItem> MenuItems{get;set;}=new List<MenuItem>();
}