using FDP.Dtos.MenuItem;

namespace FDP.Interface;

public interface IMenuItemService
{
    public Task<ViewMenuItemDto> CreateMenuItemAsync(CreateMenuItemDto dto,int userId);
    public Task<ViewMenuItemDto?> UpdateMenuItemAsync(int id, UpdateMenuItemDto dto,int userId);
    public Task<ViewMenuItemDto?> SeeMenuItemByIdAsync(int id);
    public Task<List<ViewMenuItemDto>> SeeAllMenuItems();
    public Task<string?> DeleteMenuItem(int id,int userId);

}