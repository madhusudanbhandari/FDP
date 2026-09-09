using FDP.Dtos.Restaurant;
using FDP.Interface;
using FDP.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FDP.Services;

public class RestaurantService : IRestaurantService
{
    private readonly IRestaurantRepository _restaurantRepository;

    public RestaurantService(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository=restaurantRepository;
    }

    public async Task<ViewRestaurantDto> CreateRestaurantAsync(CreateRestaurantDto dto)
    {
        var restaurant=new Restaurant
        {
            Name=dto.Name,
            Address=dto.Address,
            Capacity=dto.Capacity,
            Special=dto.Special
        };

        await _restaurantRepository.AddAsync(restaurant);
        await _restaurantRepository.SaveChangesAsync();

        return new ViewRestaurantDto
        {
            Id=restaurant.Id,
            Name=restaurant.Name,
            Address=restaurant.Address,
            Capacity=restaurant.Capacity,
            Special=restaurant.Special
        };
    }

    public async Task<ViewRestaurantDto?> UpdateRestaurantAsync(int id, UpdateRestaurantDto dto)
    {
        var restaurant=await _restaurantRepository.GetRestaurantByIdAsync(id);

        if(restaurant == null)
        {
         return null;   
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
            Special=restaurant.Special
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
        Special=r.Special
    }).ToList();      

    }

    public async Task<ViewRestaurantDto?> SeeRestaurantAsync(int id)
    {
        var restaurant= await _restaurantRepository.GetRestaurantByIdAsync(id);

        if (restaurant== null)
        {
            return null;
        }

        return new ViewRestaurantDto
        {
            Id=restaurant.Id,
            Name=restaurant.Name,
            Address=restaurant.Address,
            Capacity=restaurant.Capacity,
            Special=restaurant.Special

        };
    }
    

}