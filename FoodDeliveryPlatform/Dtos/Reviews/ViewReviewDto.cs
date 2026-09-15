namespace FDP.Dtos.Reviews;

public class ViewReviewDto
{
    public int Rating{get;set;}
    public string Comment{get;set;}=string.Empty;
    public int UserId{get;set;}
    public int RestaurantId{get;set;}
    public int OrderId{get;set;}
    public DateTime CreatedAt{get;set;}
}