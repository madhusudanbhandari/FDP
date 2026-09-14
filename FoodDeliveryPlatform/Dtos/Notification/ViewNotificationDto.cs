namespace FDP.Dtos.Notification;

public class ViewNotificationDto
{
    public int Id{get;set;}
    public string Message{get;set;}=string.Empty;
    public DateTime CreatedAt{get;set;}
    public bool IsRead{get;set;}

}