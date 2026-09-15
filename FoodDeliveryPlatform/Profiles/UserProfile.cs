using AutoMapper;
using FDP.Dtos.Cart;
using FDP.Dtos.MenuItem;
using FDP.Dtos.Notification;
using FDP.Dtos.Orders;
using FDP.Dtos.Reviews;
using FDP.Dtos.User;
using FDP.Models;

namespace FDP.Profiles;


public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User,RegisterResponseDto>();
        CreateMap<User,LoginResponseDto>()
                .ForMember(
                    dest=>dest.Token,
                    opt=>opt.Ignore()
                );
        CreateMap<MenuItem, ViewMenuItemDto>();
        CreateMap<Cart,ViewCartDto>();
        CreateMap<CartItem,ViewCartItemDto>();

        CreateMap<Order,ViewOrderDto>()
                .ForMember(
                    dest=>dest.Items,
                    opt=>opt.MapFrom(src=>src.OrderItems)
                )
                .ForMember(
                    dest=>dest.Status,
                    opt=>opt.MapFrom(src=>src.Status.ToString())
                )
                .ForMember(dest=>dest.RestaurantName,
                opt=>opt.MapFrom(src=>src.Restaurant.Name));

        CreateMap<OrderItem, ViewOrderItemDto>()
                .ForMember(
                    dest=>dest.MenuItemName,
                    opt=>opt.MapFrom(src=>src.MenuItem.Name)
                )
                .ForMember(
                    dest=>dest.SubTotal,
                    opt=>opt.MapFrom(src=>src.UnitPrice*src.Quantity)
                );
        CreateMap<Notification,ViewNotificationDto>();
        CreateMap<Review, ViewReviewDto>();
                
    }
}