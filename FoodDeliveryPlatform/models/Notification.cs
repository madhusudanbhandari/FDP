namespace FDP.Models;

public class Notification
{
    public int Id{get;set;}
    public int UserId{get;set;}
    public User User{get;set;}=null!;
    public string Message{get;set;}=string.Empty;
    public DateTime CreatedAt{get;set;}
    public bool IsRead{get;set;}

}