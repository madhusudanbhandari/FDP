using FDP.enums;
using Microsoft.AspNetCore.SignalR;

namespace FDP.Models;


public class Order
{
    public int Id{get;set;}
    public int UserId{get;set;}
    public User User{get;set;}=null!;
    public decimal TotalAmount{get;set;}
    public OrderStatus Status{get;set;}
    public DateTime CreatedAt{get;set;}
    public int RestaurantId{get;set;}
    public Restaurant Restaurant{get;set;}=null!;
    public List<OrderItem> OrderItems{get;set;}=new();
    public Payment? Payment{get;set;}
    public Delivery? Delivery{get;set;}
}