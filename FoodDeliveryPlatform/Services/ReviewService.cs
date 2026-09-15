using AutoMapper;
using FDP.Dtos.Reviews;
using FDP.Exceptions;
using FDP.Interface;
using FDP.Models;

namespace FDP.Services;

public class ReviewService:IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public ReviewService(IReviewRepository reviewRepository,
                IOrderRepository orderRepository,
                IMapper mapper)
    {
        _reviewRepository=reviewRepository;
        _orderRepository=orderRepository;
        _mapper=mapper;
    }

    public async Task<ViewReviewDto> CreateReviewAsync(int userId,int orderId,CreateReviewDto dto)
    {   
        var order=await _orderRepository.GetOrderForCustomerAsync(orderId,userId);

        if (order == null)
        {
            throw new NotFoundException("Cannot find the order");
        }

        if (order.Status != enums.OrderStatus.Delivered)
        {
            throw new BadRequestException("You can only review delivered orders");
        }

        if(dto.Rating<1 || dto.Rating > 5)
        {
            throw new BadRequestException("Rating must be between 1 and 5");
        }

        var existingReview=await _reviewRepository.GetReviewByUserAndOrderAsync(userId,orderId);


        if (existingReview != null)
        {
            throw new BadRequestException("You have already reviewed this order");
        }


        var review=new Review
        {
            RestaurantId=order.RestaurantId,
            Rating=dto.Rating,
            Comment=dto.Comment,
            UserId=userId,
            OrderId=order.Id
        };

         _reviewRepository.AddReview(review);
         await _reviewRepository.SaveChangesAsync();

         return _mapper.Map<ViewReviewDto>(review);

    }

    public async Task<List<ViewReviewDto>> GetRestaurantReviewAsync(int restaurantId)
    {
        var reviews=await _reviewRepository.GetAllReviewsByRestaurant(restaurantId);

        return _mapper.Map<List<ViewReviewDto>>(reviews);
    }
}