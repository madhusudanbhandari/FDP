namespace FDP.Models;

public class Review
{
    public int Id{get;set;}
    public int Rating{get;set;}
    public string Comment{get;set;}=string.Empty;
    public int UserId{get;set;}
    public User? User{get;set;}
    public int RestaurantId{get;set;}
    public Restaurant? Restaurant{get;set;}
    public int OrderId{get;set;}
    public Order? Order {get;set;}
    public DateTime CreatedAt{get;set;}
}