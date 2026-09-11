using FDP.Dtos.Menu;

namespace FDP.Interface;

public interface IMenuService
{
    public Task<ViewMenuDto> CreateMenuAsync(int restaurantId,int ownerId);
    // public Task<ViewMenuDto?> UpdateMenuAsync(int id, UpdateMenuDto dto,int restaurantId);
    public Task<ViewMenuDto?>GetMenuByIdAsync(int id);
    public Task<List<ViewMenuDto>> GetAllMenusAsync();
    public Task <string?> DeleteMenuAsync(int id);
}