namespace FDP.Dtos.Common;

public class PagedResponseDto<T>
{
    public List<T> Items{get;set;}=new();
    public int Page{get;set;}
    public int PageSize{get;set;}
    public int TotalCount{get;set;}
    public int TotalPages{get;set;}

}