using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Road2Door.Models;

public class NotificationCleanupService : BackgroundService
{
    private readonly IServiceProvider _services;

    public NotificationCleanupService(IServiceProvider services)
    {
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _services.CreateScope())
            {
                // Resolve the required dependencies
                var dbContext = scope.ServiceProvider.GetRequiredService<Road2DoorContext>();

                // Delete the expired rows from the "Notification" table
                DateTime threshold = DateTime.Now.AddMinutes(-10);
                dbContext.Notifications.RemoveRange(dbContext.Notifications.Where(n => n.InsertionTime < threshold));
                await dbContext.SaveChangesAsync();
            }

            // Wait for the specified interval before running the task again
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
