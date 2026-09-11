
using FDP.Data;
using FDP.Dtos.Common;
using FDP.Dtos.Restaurant;
using FDP.Interface;
using FDP.Models;
using Microsoft.EntityFrameworkCore;

namespace FDP.Repository;

public class RestaurantRepository:IRestaurantRepository
{
    private readonly AppDbContext _context;

    public RestaurantRepository(AppDbContext context)
    {
        _context=context;
    }

    public async Task<PagedResponseDto<Restaurant>> GetAllRestaurantsAsync(RestaurantQueryDto query)
    {
        IQueryable<Restaurant> restaurants= _context.Restaurants
                                .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            restaurants=restaurants.Where(r=>
            EF.Functions.ILike(
                r.Name,
                $"%{query.Search}%"
            ));
        }
        if (query.IsOpen.HasValue)
        {
            restaurants=restaurants.Where(r=>
            r.IsOpen==query.IsOpen.Value);
        }

        bool descending=string.Equals(
            query.SortOrder,
            "desc",
            StringComparison.OrdinalIgnoreCase
        );

        restaurants=query.SortBy?.ToLower() switch
        {
            "name"=>descending
                    ?restaurants.OrderByDescending(r=>r.Name)
                    :restaurants.OrderBy(r=>r.Name),
            
            "rating"=>descending
                    ?restaurants.OrderByDescending(r=>r.Rating)
                    :restaurants.OrderBy(r=>r.Rating),

                _ => restaurants.OrderBy(r=>r.Id)
        };

        int totalCount=await restaurants.CountAsync();

        query.Page=query.Page<1
                    ?1
                    :query.Page;
        
        query.PageSize=query.PageSize switch
        {
            <1=>10,
            >50=>50,
            _=> query.PageSize
        };

        int skip=(query.Page-1)*query.PageSize;

        List<Restaurant> items=await restaurants
                                .Skip(skip)
                                .Take(query.PageSize)
                                .ToListAsync();

        int totalPages=
                    (int)Math.Ceiling(
                        totalCount/(double)query.PageSize
                    );
            
        return new PagedResponseDto<Restaurant>
        {
            Items=items,
            Page=query.Page,
            PageSize=query.PageSize,
            TotalCount=totalCount,
            TotalPages=totalPages
        };

    }

    public async Task<Restaurant?> GetRestaurantByIdAsync(int id)
    {
        return await _context.Restaurants.FirstOrDefaultAsync(r=>r.Id==id);
    }

    public async Task <Restaurant?> GetRestaurantByOwnerIdAsync(int ownerId)
    {
        return await _context.Restaurants.FirstOrDefaultAsync(r=>r.OwnerId==ownerId);
    }

    public async Task AddAsync(Restaurant restaurant)
    {
        await _context.Restaurants.AddAsync(restaurant );
    }
    public async Task RemoveAsync(Restaurant restaurant)
    {
         _context.Restaurants.Remove(restaurant);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}