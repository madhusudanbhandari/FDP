
using FDP.enums;

namespace FDP.Models;

public class User
{
    public int Id{get;set;}
    public string FirstName{get;set;}=string.Empty;
    public string LastName{get;set;}=string.Empty;
    public string Email{get;set;}=string.Empty;
    public string Password{get;set;}=string.Empty;
    public string Address{get;set;}=string.Empty;
    
    public ROLES Role{get;set;}

    public ICollection<Restaurant> Restaurants{get;set;}=new List<Restaurant>();
    public ICollection<Cart> Carts{get;set;}=new List<Cart>();
    public ICollection<Order> Orders{get;set;}=new List<Order>();

}