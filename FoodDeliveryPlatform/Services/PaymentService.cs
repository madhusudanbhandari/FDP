using AutoMapper;
using FDP.Dtos.Payment;
using FDP.enums;
using FDP.Exceptions;
using FDP.Interface;
using FDP.Models;
using Microsoft.AspNetCore.Mvc;

namespace FDP.Services;


public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentProvider _paymentProvider;
    private readonly IMapper _mapper;
    public PaymentService(
         IPaymentRepository paymentRepository,
         IOrderRepository orderRepository,
         IPaymentProvider paymentProvider,
         IMapper mapper)
    {
        _paymentRepository=paymentRepository;
        _orderRepository=orderRepository;
        _paymentProvider=paymentProvider;
        _mapper=mapper;
    }

    public async Task<ViewPaymentDto> CreatePaymentAsync(int userId, CreatePaymentDto dto)
    {
        var order=await _orderRepository.GetOrderByIdAsync(dto.OrderId);

        if(order==null)
            throw new NotFoundException("You cannot pay for another user's order");

        if(order.UserId!=userId)
            throw new BadRequestException("You cannot pay for others order");

        if(order.Status!=enums.OrderStatus.Pending)
            throw new BadRequestException("This order cannot be paid fot");

        var existingPayment=_paymentRepository.GetPaymentByOrderIdAsync(dto.OrderId);

        if(existingPayment!=null)
            throw new BadRequestException("A Payment already exists for this order");

        var payment=new Payment
        {
            OrderId=order.Id,
            Amount=order.TotalAmount,
            PaymentMethod=dto.PaymentMethod,
            PaymentStatus=PaymentStatus.Pending,
            CreatedAt=DateTime.UtcNow
        };

        await _paymentRepository.AddPayment(payment);
        await _paymentRepository.SaveChangesAsync();

        var providerResult=await _paymentProvider.ProcessPaymentAsync(
            payment.Amount,
            payment.PaymentMethod.ToString()
        );

        if (providerResult.Sucess)
        {
            payment.PaymentStatus=PaymentStatus.Successfull;
            payment.TransactionId=providerResult.TransactionId;
            payment.PaidAt=DateTime.UtcNow;

            order.Status=OrderStatus.Confirmed;
        }
        else
        {
            payment.PaymentStatus=PaymentStatus.Failed;
        }

        await _paymentRepository.SaveChangesAsync();

        return _mapper.Map<ViewPaymentDto>(payment);

    }

    public async Task<ViewPaymentDto?> GetPaymentByIdAsync(int user,int id)
    {
        var payment=await _paymentRepository.GetPaymentByIdAsync(id);

        if (payment == null)
            return null;
        
        if(payment.Order.UserId!=user)
            throw new BadRequestException("You cannot access this payment");


        return _mapper.Map<ViewPaymentDto>(payment);
    }

    public async Task<ViewPaymentDto?> GetPaymentByOrderIdAsync(int userId,int orderId)
    {
        var payment= await _paymentRepository.GetPaymentByOrderIdAsync(orderId);

        if(payment==null)
            return null;
        var order=await _orderRepository.GetOrderByIdAsync(orderId);

        if(order==null|| order.UserId!=userId)
            throw new BadRequestException(
                "You cannot access this payment"
            );

        return  _mapper.Map<ViewPaymentDto>(payment);


    }



}