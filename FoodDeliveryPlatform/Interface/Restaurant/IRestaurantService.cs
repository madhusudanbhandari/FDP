
using FDP.Dtos.Common;
using FDP.Dtos.Restaurant;

namespace FDP.Interface;

public  interface IRestaurantService
{
    public Task<ViewRestaurantDto> CreateRestaurantAsync(CreateRestaurantDto dto,int ownerId);
    public Task<ViewRestaurantDto?> UpdateRestaurantAsync(int id, UpdateRestaurantDto dto,int ownerId);
    public Task<string?> DeleteRestaurantAsync(int id, int ownerId);
    public Task<ViewRestaurantDto?> SeeRestaurantAsync(int id);
    public Task<PagedResponseDto<ViewRestaurantDto>> SeeAllRestaurantsAsync(RestaurantQueryDto query);

}