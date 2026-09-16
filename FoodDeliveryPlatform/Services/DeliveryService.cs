using AutoMapper;
using FDP.Dtos.Delivery;
using FDP.enums;
using FDP.Exceptions;
using FDP.Interface;

namespace FDP.Services;

public class DeliveryService : IDeliveryService
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IAuthRepository _authRepository;
    private readonly IMapper _mapper;

    public DeliveryService(
        IDeliveryRepository deliveryRepository,
        IAuthRepository authRepository,
        IMapper mapper)
    {
        _deliveryRepository = deliveryRepository;
        _authRepository = authRepository;
        _mapper = mapper;
    }

    public async Task<ViewDeliveryDto?> GetByIdAsync(int id)
    {
        var delivery = await _deliveryRepository.GetByIdAsync(id);

        if (delivery == null)
            return null;

        return _mapper.Map<ViewDeliveryDto>(delivery);
    }

    public async Task<ViewDeliveryDto?> GetByOrderIdAsync(
        int orderId,
        int userId)
    {
        var delivery = await _deliveryRepository
            .GetByOrderIdAsync(orderId);

        if (delivery == null)
            return null;

        if (delivery.Order.UserId != userId)
            throw new BadRequestException(
                "You cannot view this delivery.");

        return _mapper.Map<ViewDeliveryDto>(delivery);
    }

    public async Task<List<ViewDeliveryDto>> GetAllAsync()
    {
        var deliveries = await _deliveryRepository.GetAllAsync();

        return _mapper.Map<List<ViewDeliveryDto>>(deliveries);
    }

    public async Task<List<ViewDeliveryDto>> GetMyDeliveriesAsync(
        int deliveryPersonId)
    {
        var deliveries = await _deliveryRepository
            .GetByDeliveryPersonIdAsync(deliveryPersonId);

        return _mapper.Map<List<ViewDeliveryDto>>(deliveries);
    }

    public async Task AssignDeliveryAsync(
        int deliveryId,
        int deliveryPersonId)
    {
        var delivery = await _deliveryRepository
            .GetByIdAsync(deliveryId);

        if (delivery == null)
            throw new NotFoundException(
                "Delivery not found.");

        if (delivery.DeliveryStatus != DeliveryStatus.Pending)
            throw new BadRequestException(
                "Only pending deliveries can be assigned.");

        var user = await _authRepository
            .GetByIdAsync(deliveryPersonId);

        if (user == null)
            throw new NotFoundException(
                "Delivery person not found.");

        if (user.Role !=ROLES.DeliveryPerson)
            throw new BadRequestException(
                "The selected user is not a delivery person.");

        delivery.DeliveryPersonId = deliveryPersonId;
        delivery.DeliveryStatus = DeliveryStatus.Assigned;
        delivery.AssignedAt = DateTime.UtcNow;

        await _deliveryRepository.SaveChangesAsync();
    }

    public async Task MarkPickedUpAsync(
        int deliveryId,
        int deliveryPersonId)
    {
        var delivery = await GetDeliveryForPerson(
            deliveryId,
            deliveryPersonId);

        if (delivery.DeliveryStatus != DeliveryStatus.Assigned)
            throw new BadRequestException(
                "Delivery must be assigned before pickup.");

        delivery.DeliveryStatus = DeliveryStatus.PickedUp;
        delivery.PickedUpAt = DateTime.UtcNow;

        await _deliveryRepository.SaveChangesAsync();
    }

    public async Task MarkOutForDeliveryAsync(
        int deliveryId,
        int deliveryPersonId)
    {
        var delivery = await GetDeliveryForPerson(
            deliveryId,
            deliveryPersonId);

        if (delivery.DeliveryStatus != DeliveryStatus.PickedUp)
            throw new BadRequestException(
                "Delivery must be picked up first.");

        delivery.DeliveryStatus = DeliveryStatus.OutForDelivery;

        await _deliveryRepository.SaveChangesAsync();
    }

    public async Task MarkDeliveredAsync(
        int deliveryId,
        int deliveryPersonId)
    {
        var delivery = await GetDeliveryForPerson(
            deliveryId,
            deliveryPersonId);

        if (delivery.DeliveryStatus != DeliveryStatus.OutForDelivery)
            throw new BadRequestException(
                "Delivery must be out for delivery first.");

        delivery.DeliveryStatus = DeliveryStatus.Delivered;
        delivery.DeliveredAt = DateTime.UtcNow;

        delivery.Order.Status = OrderStatus.Delivered;

        await _deliveryRepository.SaveChangesAsync();
    }

    private async Task<Models.Delivery> GetDeliveryForPerson(
        int deliveryId,
        int deliveryPersonId)
    {
        var delivery = await _deliveryRepository
            .GetByIdAsync(deliveryId);

        if (delivery == null)
            throw new NotFoundException(
                "Delivery not found.");

        if (delivery.DeliveryPersonId != deliveryPersonId)
            throw new BadRequestException(
                "This delivery is not assigned to you.");

        return delivery;
    }
}