using FDP.Dtos.Menu;
using FDP.Exceptions;
using FDP.Interface;
using FDP.Repository;

namespace FDP.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;
    private readonly IRestaurantRepository _restaurantRepository;
     private readonly IRedisService _redis;
    public MenuService(IMenuRepository menuRepository,IRestaurantRepository restaurantRepository,IRedisService redis)
    {
        _menuRepository=menuRepository;
        _restaurantRepository=restaurantRepository;
        _redis=redis;
    }

    public async Task <ViewMenuDto> CreateMenuAsync(int restaurantId,int ownerId)
    {
        var restaurant=await _restaurantRepository.GetRestaurantByIdAsync(restaurantId);

        if (restaurant == null)
        {
            throw new NotFoundException("Cannot find the restaurant");
        }       

        if (restaurant.OwnerId != ownerId)
        {
            throw new InvalidOperationException("You do not own this restaurant");
        }

        var menu=new Menu
        {
            RestaurantId=restaurantId        
        };

         await _menuRepository.AddAsync(menu);
         await _menuRepository.SaveChangesAsync();

         return new ViewMenuDto
         {
             Id=menu.Id,
             RestaurantId=menu.RestaurantId

         };

         
    }

    // public async Task<ViewMenuDto?> UpdateMenuAsync(int id, UpdateMenuDto dto,int restaurantId)
    // {
    //     var menu=await _menuRepository.GetMenuByIdAsync(id);
    //     if (menu == null)
    //     {
    //         throw new NotFoundException("Cannot find the menu");
    //     }

    //     if (menu.RestaurantId != restaurantId)
    //     {
    //        throw new BadRequestException("This is not your menu"); 
    //     }

    //     menu.Category=dto.Category;
    //     menu.ItemName=dto.ItemName;
    //     menu.ItemPrice=dto.ItemPrice;
    //     menu.quantity=dto.quantity;

    //     return new ViewMenuDto
    //      {
    //          Id=menu.Id,
    //          Category=menu.Category,
    //          ItemName=menu.ItemName,
    //          ItemPrice=menu.ItemPrice,
    //          quantity=menu.quantity,
    //          RestaurantId=menu.RestaurantId

    //      };
    // }

    public async Task<ViewMenuDto?> GetMenuByIdAsync(int id)
    {
        var cacheKey=$"fdp:restaurant:{id}:menu";

        var cachedMenu=await _redis.GetAsync<ViewMenuDto?>(cacheKey);

        if(cachedMenu is not null)
        {
            return cachedMenu;
        }

        var menu=await _menuRepository.GetMenuByIdAsync(id);

        if (menu == null)
        {
            throw new NotFoundException("Cannot find the menu");
        }

        await _redis.SetAsync(cacheKey,menu,TimeSpan.FromMinutes(5));

        return new ViewMenuDto
         {
             Id=menu.Id,
             RestaurantId=menu.RestaurantId

         };
    }

    public async Task<List<ViewMenuDto>> GetAllMenusAsync()
    {
        string cacheKey=$"fdp:restaurant:menu";

        var cachedMenu=await _redis.GetAsync<List<ViewMenuDto>>(cacheKey);

        if(cachedMenu is not null)
        {
            return cachedMenu;
        }

        var menus=await _menuRepository.GetAllMenusAsync();

        await _redis.SetAsync(cacheKey,menus,TimeSpan.FromMinutes(10));

        return menus.Select(menu=>new ViewMenuDto
         {
             Id=menu.Id,
             RestaurantId=menu.RestaurantId

         }).ToList();
    }

    public async Task<string?> DeleteMenuAsync(int id,int ownerId)
    {
        var menu=await _menuRepository.GetMenuByIdAsync(id);

        if (menu == null)
        {
            throw new NotFoundException("Cannot find the menu");
        }
        if (menu.Restaurant == null)
        {
            throw new NotFoundException("Cannot find the restaurant");
        }
        if (menu.Restaurant.OwnerId != ownerId)
        {
            throw new InvalidOperationException("You did not own this restaurant");
        }

        await _menuRepository.RemoveAsync(menu);
        await _menuRepository.SaveChangesAsync();

        return "Deleted successfully";
    }


}