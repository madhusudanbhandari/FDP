using FDP.Data;
using FDP.Dtos.Common;
using FDP.Dtos.Restaurant;
using FDP.Models;

namespace FDP.Interface;

public interface IRestaurantRepository
{

    Task<PagedResponseDto<Restaurant>> GetAllRestaurantsAsync(RestaurantQueryDto query);
    Task<Restaurant?> GetRestaurantByIdAsync(int id);
    Task<Restaurant?> GetRestaurantByOwnerIdAsync(int ownerId);
    Task AddAsync(Restaurant restaurant);
    Task RemoveAsync(Restaurant restaurant);
    Task SaveChangesAsync();
    


}