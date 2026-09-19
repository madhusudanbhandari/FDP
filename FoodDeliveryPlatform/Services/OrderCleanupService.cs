using FDP.enums;
using FDP.Interface;

namespace FDP.Services;

public class OrderCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OrderCleanupService> _logger;

    public OrderCleanupService(IServiceScopeFactory scopeFactory, 
                ILogger<OrderCleanupService> logger)
    {
        _scopeFactory=scopeFactory;
        _logger=logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken
    )
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope=_scopeFactory.CreateScope();

                var orderRepository=scope.ServiceProvider
                                    .GetRequiredService<IOrderRepository>();

                var expiredOrders=await orderRepository.GetExpiredPendingOrdersAsync();

                foreach(var order in expiredOrders)
                {
                    order.Status=OrderStatus.Cancelled;

                    _logger.LogInformation("Order {OrderId} cancelled because payment was not collected",order.Id);
                }

                if (expiredOrders.Count > 0)
                {
                    await orderRepository.SaveChangesAsync();
                }

                await Task.Delay(
                    TimeSpan.FromMinutes(5),
                    stoppingToken
                );
            }catch(Exception ex)
            {
                _logger.LogError(ex,
                "Error while running order cleanup");

                await Task.Delay(
                    TimeSpan.FromSeconds(30),
                    stoppingToken
                );
            }
        }
    }
}