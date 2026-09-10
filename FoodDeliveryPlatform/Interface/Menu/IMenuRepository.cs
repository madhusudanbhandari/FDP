namespace FDP.Interface;

public interface IMenuRepository
{
    Task<Menu?> GetMenuByIdAsync(int id);
    Task<List<Menu>> GetAllMenusAsync();

    Task AddAsync(Menu menu);
    Task RemoveAsync(Menu menu);
    Task SaveChangesAsync();
}