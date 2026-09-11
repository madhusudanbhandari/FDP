namespace FDP.Dtos.MenuItem;

public class UpdateMenuItemDto
{
    public string Category{get;set;}=string.Empty;
    public string Name{get;set;}=string.Empty;
    public bool IsAvailable{get;set;}
    public decimal Price{get;set;}

    public int MenuId{get;set;}

}