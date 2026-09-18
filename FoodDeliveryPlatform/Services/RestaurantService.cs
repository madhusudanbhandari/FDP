using FDP.Dtos.Common;
using FDP.Dtos.Restaurant;
using FDP.Exceptions;
using FDP.Interface;
using FDP.Models;

namespace FDP.Services;

public class RestaurantService : IRestaurantService
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IRedisService _redis;

    public RestaurantService(IRestaurantRepository restaurantRepository,IRedisService redis)
    {
        _restaurantRepository=restaurantRepository;
        _redis=redis;
    }

    public async Task<ViewRestaurantDto> CreateRestaurantAsync(CreateRestaurantDto dto,int ownerId)
    {
        var restaurant=new Restaurant
        {
            Name=dto.Name,
            Address=dto.Address,
            Capacity=dto.Capacity,
            Special=dto.Special,
            IsOpen=dto.IsOpen,
            Rating=dto.Rating,
            OwnerId=ownerId
        };

        await _restaurantRepository.AddAsync(restaurant);
        await _restaurantRepository.SaveChangesAsync();

        return new ViewRestaurantDto
        {
            Id=restaurant.Id,
            Name=restaurant.Name,
            Address=restaurant.Address,
            Capacity=restaurant.Capacity,
            Special=restaurant.Special,
            ownerId=restaurant.OwnerId,
            IsOpen=restaurant.IsOpen,
            Rating=restaurant.Rating,
            
        };
    }

    public async Task<ViewRestaurantDto?> UpdateRestaurantAsync(int id, UpdateRestaurantDto dto,int ownerId)
    {
        var restaurant=await _restaurantRepository.GetRestaurantByIdAsync(id);

        if(restaurant == null)
        {
            throw new NotFoundException("Restaurant not found");
        }

        if (restaurant.OwnerId != ownerId)
        {
            throw new UnauthorizedAccessException("You dont own this restaurant");
        }

        restaurant.Name=dto.Name;
        restaurant.Address=dto.Address;
        restaurant.Capacity=dto.Capacity;
        restaurant.Special=dto.Special;
        restaurant.IsOpen=dto.IsOpen;
        restaurant.Rating=dto.Rating;

        await _restaurantRepository.SaveChangesAsync();

        return new ViewRestaurantDto
        {
            Id=restaurant.Id,
            Name=restaurant.Name,
            Address=restaurant.Address,
            Capacity=restaurant.Capacity,
            Special=restaurant.Special,
            ownerId=restaurant.OwnerId,
            IsOpen=restaurant.IsOpen,
            Rating=restaurant.Rating,
        };
    }

    public async Task<PagedResponseDto<ViewRestaurantDto>> SeeAllRestaurantsAsync(RestaurantQueryDto query)
    {
        string cacheKey=$"fdp:restaurants:"+
                        $"p{query.Page}:"+
                        $"ps{query.PageSize}:"+
                        $"search={query.Search}:"+
                        $"open={query.IsOpen}:"+
                        $"sort={query.SortBy}:"+
                        $"order={query.SortOrder}";

        //will get fdp:restaurants:p1:ps10:search=momoshop:open=True:sort=rating:order=desc

        //it will check for cache hit
        var cachedRestaurants=await _redis.GetAsync<PagedResponseDto<ViewRestaurantDto>>(cacheKey);

        if(cachedRestaurants is not null)
            {
                return cachedRestaurants;
            }

        //for cache miss
        var restaurants=await _restaurantRepository.
                        GetAllRestaurantsAsync(query);

        
        var restaurantDtos= restaurants.Items.Select(r =>new ViewRestaurantDto
        {
            Id=r.Id,
            Name=r.Name,
            Address=r.Address,
            Capacity=r.Capacity,
            Special=r.Special,
            IsOpen=r.IsOpen,
            Rating=r.Rating,
            ownerId=r.OwnerId
        }).ToList();    

        var response= new PagedResponseDto<ViewRestaurantDto>
        {
            Items=restaurantDtos,
            Page=restaurants.Page,
            PageSize=restaurants.PageSize,
            TotalCount=restaurants.TotalCount,
            TotalPages=restaurants.TotalPages
        };

        await _redis.SetAsync(cacheKey,response);

        return response;

        }

    public async Task<ViewRestaurantDto?> SeeRestaurantAsync(int id)
    {
        var restaurant= await _restaurantRepository.GetRestaurantByIdAsync(id);

        if (restaurant== null)
        {
            throw new NotFoundException("Restaurant not found");
        }

        return new ViewRestaurantDto
        {
            Id=restaurant.Id,
            Name=restaurant.Name,
            Address=restaurant.Address,
            Capacity=restaurant.Capacity,
            Special=restaurant.Special,
            IsOpen=restaurant.IsOpen,
            Rating=restaurant.Rating,
            ownerId=restaurant.OwnerId

        };
    }
    public async Task<string?> DeleteRestaurantAsync(int id,int ownerId)
    {
        var restaurant=await _restaurantRepository.GetRestaurantByIdAsync(id);

        if (restaurant == null)
        {
            throw new NotFoundException("Restaurant not found");
        }

        if (restaurant.OwnerId != ownerId)
        {
            throw new UnauthorizedAccessException("You dont own this restaurant");
        }

        await _restaurantRepository.RemoveAsync(restaurant);
        await _restaurantRepository.SaveChangesAsync();

        return "Deleted successfully";
    }
    

}