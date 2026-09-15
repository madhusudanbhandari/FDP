using FDP.Data;
using FDP.Interface;
using FDP.Models;
using Microsoft.EntityFrameworkCore;

namespace FDP.Repository;


public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _context;

    public ReviewRepository(AppDbContext context)
    {
        _context=context;
    }

    public async Task<Review?>GetReviewByUserAndOrderAsync(int userId,int orderId)
    {
        return await _context.Reviews.
                        FirstOrDefaultAsync(r=>
                        r.UserId==userId && 
                        r.OrderId==orderId);
    }

    public async Task<List<Review>> GetAllReviewsByRestaurant(int restaurantId)
    {
        return await _context.Reviews
                    .Where(r=>r.RestaurantId==restaurantId)
                    .OrderByDescending(r=>r.CreatedAt)
                    .ToListAsync();
    }
    public  void AddReview(Review review)
    {
         _context.Reviews.Add(review);
    }

    public void RemoveReview(Review review)
    {
        _context.Reviews.Remove(review);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}