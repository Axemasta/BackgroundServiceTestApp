using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BackgroundServiceTestApp.Queue;

public class QueuedHostedService(
    IBackgroundTaskQueue backgroundTaskQueue, 
    ILogger<QueuedHostedService> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("""
                              {Name} is running.
                              Any items in the queue will be processed
                              """,
            nameof(QueuedHostedService));

        return ProcessTaskQueueAsync(stoppingToken);
    }
    
    private async Task ProcessTaskQueueAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                Func<CancellationToken, ValueTask>? workItem =
                    await backgroundTaskQueue.DequeueAsync(stoppingToken);

                await workItem(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Prevent throwing if stoppingToken was signaled
                logger.LogInformation("Processing queue was cancelled");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred executing task work item");
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            $"{nameof(QueuedHostedService)} is stopping.");

        await base.StopAsync(stoppingToken);
    }
}