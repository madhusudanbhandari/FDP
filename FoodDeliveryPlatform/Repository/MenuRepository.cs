using FDP.Data;
using FDP.Interface;
using Microsoft.EntityFrameworkCore;

namespace FDP.Repository;

public class MenuRepository : IMenuRepository
{
    private readonly AppDbContext _context;

    public MenuRepository(AppDbContext context)
    {
        _context=context;
    }

    public async Task<Menu?> GetMenuByIdAsync(int id)
    {
        return await _context.Menus.
        Include(m=>m.Restaurant).
        FirstOrDefaultAsync(m=>m.Id==id);
    }

    public async Task<List<Menu>> GetAllMenusAsync()
    {
        return await _context.Menus.ToListAsync();
    }

    public async Task AddAsync(Menu menu)
    {
        await _context.Menus.AddAsync(menu);
    }

    public async Task RemoveAsync(Menu menu)
    {
         _context.Menus.Remove(menu);
    }

    public  async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}