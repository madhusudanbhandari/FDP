using System.Net.NetworkInformation;

namespace FDP.Models;

public class Restaurant
{
    public int Id{get;set;}
    public string Name{get;set;}=string.Empty;
    public string Address{get;set;}=string.Empty;
    public bool? IsOpen{get;set;}
    public double Rating{get;set;}
    public int Capacity{get;set;}
    public string Special{get;set;}=string.Empty;

    public int OwnerId{get;set;}
    public User? RestaurantOwner{get;set;}

    public Menu? Menu{get;set;}

    public ICollection<Review> Reviews{get;set;}=new List<Review>();
}