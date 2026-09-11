using FDP.Data;
using FDP.Interface;
using FDP.Models;
using Microsoft.EntityFrameworkCore;

namespace FDP.Repository;

public class MenuItemRepository : IMenuItemRepository
{
    private readonly AppDbContext _context;
    public MenuItemRepository(AppDbContext context)
    {
        _context=context;
    }


    public async Task<MenuItem?> GetMenuItemByIdAsync(int id)
    {
        return await _context.MenuItems
                    .FirstOrDefaultAsync(mi=>mi.Id==id);
    }

    public async Task<List<MenuItem>> GetAllMenuItemsAsync()
    {
        return  await _context.MenuItems.ToListAsync();
    }

    public async Task<Menu?> GetMenuWithRestaurantAsync(int menuId)
    {
        return await _context.Menus
                .Include(m=>m.Restaurant)
                .FirstOrDefaultAsync(m=>m.Id==menuId);
    }

    public async Task AddAsync(MenuItem menuItem)
    {
         _context.MenuItems.Add(menuItem);
    }

    public async Task Remove(MenuItem menuItem)
    {
        _context.MenuItems.Remove(menuItem);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}