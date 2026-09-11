using FDP.Models;

namespace FDP.Interface;

public interface IMenuItemRepository
{
    Task<MenuItem?> GetMenuItemByIdAsync(int id);
    Task<List<MenuItem>> GetAllMenuItemsAsync();

    Task<Menu?>GetMenuWithRestaurantAsync(int menuId);
    
    Task AddAsync(MenuItem menuItem);
    Task Remove(MenuItem menuItem);
    Task SaveChangesAsync();
}