using FDP.Data;
using FDP.Models;

namespace FDP.Interface;

public interface IRestaurantRepository
{

    Task<List<Restaurant>> GetAllRestaurantsAsync();
    Task<Restaurant?> GetRestaurantByIdAsync(int id);
    Task<Restaurant?> GetRestaurantByOwnerIdAsync(int ownerId);
    Task AddAsync(Restaurant restaurant);
    Task RemoveAsync(Restaurant restaurant);
    Task SaveChangesAsync();
    


}