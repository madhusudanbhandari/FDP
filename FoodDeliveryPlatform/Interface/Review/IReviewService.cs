using FDP.Dtos.Reviews;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FDP.Services;

public interface IReviewService
{
    public  Task<ViewReviewDto> CreateReviewAsync(int userId, int orderId,CreateReviewDto dto);
    public Task<List<ViewReviewDto>> GetRestaurantReviewAsync(int restaurantId);

}