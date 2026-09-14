using AutoMapper;
using FDP.Data;
using FDP.Dtos.Orders;
using FDP.Exceptions;
using FDP.Interface;
using FDP.Models;
using Microsoft.EntityFrameworkCore;

namespace FDP.Services;

public class OrderService:IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public OrderService(IOrderRepository orderRepository,
        ICartRepository cartRepository,
        AppDbContext context,
        IRestaurantRepository restaurantRepository,
        IMapper mapper
        )
    {
        _orderRepository=orderRepository;
        _cartRepository=cartRepository;
        _restaurantRepository=restaurantRepository;
        _context=context;
        _mapper=mapper;

    }

   public async Task<ViewOrderDto> CreateOrderAsync(int userId)
    {
        var cart=await _cartRepository.GetCartByUserIdAsync(userId);

        if (cart == null)
        {
            throw new NotFoundException("Cannot find the cart");
        }

        if (!cart.CartItems.Any())
        {
            throw new BadRequestException("Your cart is empty");
        }

        var restaurantId=cart.CartItems
                .Select(ci=>ci.MenuItem.Menu!.RestaurantId)
                .Distinct()
                .Single();
        
       

        await using var transaction=await _context.Database.BeginTransactionAsync();

        try
        {
            var order=new Order
            {
                UserId=userId,
                RestaurantId=restaurantId,
                Status=enums.OrderStatus.Pending,
                CreatedAt=DateTime.UtcNow,
                TotalAmount=0
            };

            foreach(var cartItem in cart.CartItems)
            {
                var menuItem=await _context.MenuItems
                    .FirstOrDefaultAsync(m=>m.Id==cartItem.MenuItemId);

                if (menuItem == null)
                {
                    throw new NotFoundException(
                        $"Menu item with ID{cartItem.MenuItemId} was not found"
                    );
                }

                if(!menuItem.IsAvailable)
                    throw new BadRequestException($"{menuItem.Name}is currently unaavailable");

                var orderItem=new OrderItem
                {
                    MenuItemId=menuItem.Id,
                    Quantity=cartItem.Quantity,
                    UnitPrice=menuItem.Price,
                    MenuItem=menuItem
                };

                order.OrderItems.Add(orderItem);

                order.TotalAmount+=menuItem.Price*cartItem.Quantity;
            }
                _context.Orders.Add(order);

                await _context.SaveChangesAsync();
                _context.Carts.Remove(cart);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                
                return _mapper.Map<ViewOrderDto>(order);
            
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public async Task<ViewOrderDto?> GetOrderByIdAsync
        (int orderId,
        int userId)
    {
        var order=await _orderRepository.GetOrderByIdAsync(orderId);

        if(order==null)
            return null;

        if (order.UserId != userId)
        {
            throw new BadRequestException("You cannot access this order");
        }

        return _mapper.Map<ViewOrderDto>(order);
    }

    public async Task<List<ViewOrderDto>> GetMyOrdersAsync(int userId)
    {
        var orders=await _orderRepository.GetOrderByUserIdAsync(userId);

        return _mapper.Map<List<ViewOrderDto>>(orders);
    }

    public async Task<List<ViewOrderDto>> GetOrdersOfMyRestaurant(int userId)
    {
        var restaurant=await _restaurantRepository.GetRestaurantByOwnerIdAsync(userId);

        if(restaurant==null)
            throw new NotFoundException("Restaurant Not found");

        var orders=await _orderRepository.GetOrdersByRestaurantId(restaurant.Id);

        return _mapper.Map<List<ViewOrderDto>>(orders);
        

    }

    public async Task<ViewOrderDto?> UpdateOrderStatus(int userId,int orderId,UpdateOrderStatusDto dto)
    {
        var order=await _orderRepository.GetOrderForOwnerAsync(orderId,userId);

        if(order==null)
            throw new NotFoundException("Cannot find the order");

        order.Status=dto.orderStatus;

        await _orderRepository.SaveChangesAsync();

        return  _mapper.Map<ViewOrderDto>(order);
        
    }

    public async Task CancelOrderAsync(int orderId, int userId)
    {
        var order=await _orderRepository.GetOrderForCustomerAsync(orderId, userId);

        if(order==null)
            throw new NotFoundException("Cannot find the oder");

        if(order.Status!=enums.OrderStatus.Pending &&
            order.Status != enums.OrderStatus.Confirmed)
        {
            throw new BadRequestException("This order cannot be canceled");
        }

        order.Status=enums.OrderStatus.Cancelled;
        await _orderRepository.SaveChangesAsync();

    }

}