namespace FDP.Dtos.Restaurant;

public class RestaurantQueryDto
{
    public int Page{get;set;}=1;
    public int PageSize{get;set;}=10;

    public string? Search{get;set;}
    public bool? IsOpen{get;set;}
    public string? SortBy{get;set;}
    public string? SortOrder{get;set;}
}