using FDP.Data;
using FDP.Models;

namespace FDP.Interface;

public interface IRestaurantRepository
{

    Task<List<Restaurant>> GetAllRestaurantsAsync();
    Task<Restaurant?> GetRestaurantByIdAsync(int id);

    Task AddAsync(Restaurant restaurant);
    Task SaveChangesAsync();
    


}