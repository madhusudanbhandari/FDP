namespace FDP.Dtos.Reviews;

public class CreateReviewDto
{
    public int Rating{get;set;}
    public string Comment{get;set;}=string.Empty;
    public DateTime CreatedAt{get;set;}
}