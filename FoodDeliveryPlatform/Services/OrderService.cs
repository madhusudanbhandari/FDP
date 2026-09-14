using AutoMapper;
using FDP.Data;
using FDP.Dtos.Orders;
using FDP.Exceptions;
using FDP.Interface;
using FDP.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FDP.Services;

public class OrderService:IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public OrderService(IOrderRepository orderRepository,
        ICartRepository cartRepository,
        AppDbContext context,
        IMapper mapper
        )
    {
        _orderRepository=orderRepository;
        _cartRepository=cartRepository;
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
        await using var transaction=await _context.Database.BeginTransactionAsync();

        try
        {
            var order=new Order
            {
                UserId=userId,
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

}