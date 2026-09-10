using FDP.Dtos.Menu;
using FDP.Exceptions;
using FDP.Interface;
using FDP.Repository;

namespace FDP.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;
    public MenuService(IMenuRepository menuRepository)
    {
        _menuRepository=menuRepository;
    }

    public async Task <ViewMenuDto> CreateMenuAsync(CreateMenuDto dto,int restaurantId)
    {
        var menu=new Menu
        {
            Category=dto.Category,
            ItemName=dto.ItemName,
            ItemPrice=dto.ItemPrice,
            quantity=dto.quantity,
            RestaurantId=restaurantId        
        };

         await _menuRepository.AddAsync(menu);
         await _menuRepository.SaveChangesAsync();

         return new ViewMenuDto
         {
             Id=menu.Id,
             Category=menu.Category,
             ItemName=menu.ItemName,
             ItemPrice=menu.ItemPrice,
             quantity=menu.quantity,
             RestaurantId=menu.RestaurantId

         };

         
    }

    public async Task<ViewMenuDto?> UpdateMenuAsync(int id, UpdateMenuDto dto,int restaurantId)
    {
        var menu=await _menuRepository.GetMenuByIdAsync(id);
        if (menu == null)
        {
            throw new NotFoundException("Cannot find the menu");
        }

        if (menu.RestaurantId != restaurantId)
        {
           throw new BadRequestException("This is not your menu"); 
        }

        menu.Category=dto.Category;
        menu.ItemName=dto.ItemName;
        menu.ItemPrice=dto.ItemPrice;
        menu.quantity=dto.quantity;

        return new ViewMenuDto
         {
             Id=menu.Id,
             Category=menu.Category,
             ItemName=menu.ItemName,
             ItemPrice=menu.ItemPrice,
             quantity=menu.quantity,
             RestaurantId=menu.RestaurantId

         };
    }

    public async Task<ViewMenuDto?> GetMenuByIdAsync(int id)
    {
        var menu=await _menuRepository.GetMenuByIdAsync(id);

        if (menu == null)
        {
            throw new NotFoundException("Cannot find the menu");
        }

        return new ViewMenuDto
         {
             Id=menu.Id,
             Category=menu.Category,
             ItemName=menu.ItemName,
             ItemPrice=menu.ItemPrice,
             quantity=menu.quantity,
             RestaurantId=menu.RestaurantId

         };
    }

    public async Task<List<ViewMenuDto>> GetAllMenusAsync()
    {
        var menus=await _menuRepository.GetAllMenusAsync();

        return menus.Select(menu=>new ViewMenuDto
         {
             Id=menu.Id,
             Category=menu.Category,
             ItemName=menu.ItemName,
             ItemPrice=menu.ItemPrice,
             quantity=menu.quantity,
             RestaurantId=menu.RestaurantId

         }).ToList();
    }

    public async Task<string?> DeleteMenuAsync(int id)
    {
        var menu=await _menuRepository.GetMenuByIdAsync(id);

        if (menu == null)
        {
            throw new NotFoundException("Cannot find the exception");
        }

        await _menuRepository.RemoveAsync(menu);
        await _menuRepository.SaveChangesAsync();

        return "Deleted successfully";
    }


}