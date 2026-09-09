
using FDP.Dtos.Restaurant;

namespace FDP.Interface;

public  interface IRestaurantService
{
    public Task<ViewRestaurantDto> CreateRestaurantAsync(CreateRestaurantDto dto);
    public Task<ViewRestaurantDto?> UpdateRestaurantAsync(int id, UpdateRestaurantDto dto);
    public Task<ViewRestaurantDto?> SeeRestaurantAsync(int id);
    public Task<List<ViewRestaurantDto>> SeeAllRestaurantsAsync();

}