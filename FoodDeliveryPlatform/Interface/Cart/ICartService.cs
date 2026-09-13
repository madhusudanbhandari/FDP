using FDP.Dtos;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FDP.Interface;

public interface ICartService
{
    public Task<ViewCartDto> AddToCartAsync(int userId,AddToCartDto dto);
    public  Task<ViewCartDto> UpdateCartAsync(
        int userId,
        int cartItemId,
        UpdateCartItemDto dto
    ) ;
    public Task<ViewCartDto> ViewMyCartAsync(int userId);
    public Task<ViewCartDto> RemoveCartItemAsync(int userId,int CartItemId);
}