using FDP.Dtos.MenuItem;
using FDP.Interface;
using FDP.Models;
using AutoMapper;
using FDP.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FDP.Services;

public class MenuItemService : IMenuItemService
{
    private readonly IMenuItemRepository _menuItemRepository;
    private readonly IMapper _mapper;
    public MenuItemService(IMenuItemRepository menuItemRepository,IMapper mapper)
    {
        _menuItemRepository=menuItemRepository;
        _mapper=mapper;
    }

    public async Task<ViewMenuItemDto> CreateMenuItemAsync(CreateMenuItemDto dto,int userId)
    {
        var menu=await _menuItemRepository
            .GetMenuWithRestaurantAsync(dto.MenuId);

        if (menu == null)
        {
            throw new NotFoundException("Menu not found");
        }
        if (menu.Restaurant == null)
        {
            throw new NotFoundException("Restaurant not found");
        }
        if (menu.Restaurant.OwnerId != userId)
        {
            throw new InvalidOperationException("You donot own this restaurant");
        }

        var menuItem=new MenuItem
        {
            Name=dto.Name,
            Category=dto.Category,
            IsAvailable=dto.IsAvailable,
            Price=dto.Price,    
            MenuId=dto.MenuId       
        };

        await _menuItemRepository.AddAsync(menuItem);
        await _menuItemRepository.SaveChangesAsync();

        return _mapper.Map<ViewMenuItemDto>(menuItem);
    }

    public async Task<ViewMenuItemDto?> UpdateMenuItemAsync(int id, UpdateMenuItemDto dto,int userId)
    {
        var menuItem=await _menuItemRepository.GetMenuItemByIdAsync(id);

        if (menuItem == null)
        {
            return null;
        }

        var menu=await _menuItemRepository
            .GetMenuWithRestaurantAsync(menuItem.MenuId);

        if (menu == null)
        {
            throw new NotFoundException("Menu not found");
        }

        if (menu.Restaurant == null)
        {
            throw new NotFoundException("Restaurant not found");
        }

        if (menu.Restaurant.OwnerId != userId)
        {
            throw new InvalidOperationException("You donot own this restaurant");
        }

        menuItem.Name=dto.Name;
        menuItem.Category=dto.Category;
        menuItem.IsAvailable=dto.IsAvailable;
        menuItem.Price=dto.Price;

        await _menuItemRepository.SaveChangesAsync();

        return _mapper.Map<ViewMenuItemDto>(menuItem);
    }

    public async Task<ViewMenuItemDto?> SeeMenuItemByIdAsync(int id)
    {
        var menuItem=await _menuItemRepository.GetMenuItemByIdAsync(id);

        if(menuItem==null)
            return null;

        return _mapper.Map<ViewMenuItemDto> (menuItem);
    }

    public async Task<List<ViewMenuItemDto>> SeeAllMenuItems()
    {
        var menuItems=await _menuItemRepository.GetAllMenuItemsAsync();

         return _mapper.Map<List<ViewMenuItemDto>>(menuItems);
    }

    public async Task<string?> DeleteMenuItem(int id,int userId)
    {

        var menuItem=await _menuItemRepository.GetMenuItemByIdAsync(id);

        if (menuItem == null)
        {
            return null;
        }


        var menu=await _menuItemRepository
        .GetMenuWithRestaurantAsync(menuItem.MenuId);

        if (menu == null)
        {
            throw new NotFoundException("Menu not found");
        }
        if (menu.Restaurant == null)
        {
            throw new NotFoundException("Restaurant not found");
        }
        if (menu.Restaurant.OwnerId != userId)
        {
            throw new InvalidOperationException("You donot own this restaurant");
        }

        await _menuItemRepository.Remove(menuItem);
        return "Deleted successfully";
    }
}