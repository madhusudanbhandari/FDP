using FDP.Dtos.Restaurant;
using FDP.Exceptions;
using FDP.Interface;
using FDP.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FDP.Services;

public class RestaurantService : IRestaurantService
{
    private readonly IRestaurantRepository _restaurantRepository;

    public RestaurantService(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository=restaurantRepository;
    }

    public async Task<ViewRestaurantDto> CreateRestaurantAsync(CreateRestaurantDto dto,int ownerId)
    {
        var restaurant=new Restaurant
        {
            Name=dto.Name,
            Address=dto.Address,
            Capacity=dto.Capacity,
            Special=dto.Special,
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

        await _restaurantRepository.SaveChangesAsync();

        return new ViewRestaurantDto
        {
            Id=restaurant.Id,
            Name=restaurant.Name,
            Address=restaurant.Address,
            Capacity=restaurant.Capacity,
            Special=restaurant.Special,
            ownerId=restaurant.OwnerId
        };
    }

    public async Task<List<ViewRestaurantDto>> SeeAllRestaurantsAsync()
    {

    var restaurants=await _restaurantRepository.GetAllRestaurantsAsync();

    return restaurants.Select(r =>new ViewRestaurantDto
    {
        Id=r.Id,
        Name=r.Name,
        Address=r.Address,
        Capacity=r.Capacity,
        Special=r.Special,
        ownerId=r.OwnerId
    }).ToList();      

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