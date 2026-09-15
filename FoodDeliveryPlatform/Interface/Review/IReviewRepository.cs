using FDP.Models;

namespace FDP.Interface;

public interface IReviewRepository
{
    Task<Review?> GetReviewByUserAndOrderAsync(int userId,int orderId);
    Task<List<Review>> GetAllReviewsByRestaurant(int restaurantId);
    void AddReview(Review review);
    void RemoveReview(Review review);
    Task SaveChangesAsync();
}