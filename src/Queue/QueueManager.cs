using Microsoft.Extensions.Logging;

namespace BackgroundServiceTestApp.Queue;

// This was know as MonitorLoop in the tutorial
public sealed class QueueManager(
    IBackgroundTaskQueue taskQueue,
    QueuedHostedService queuedHostedService,
    ILogger<QueueManager> logger)
{
    private readonly CancellationTokenSource _hostCancellationTokenSource = new();

    public async Task StartQueue()
    {
        await queuedHostedService.StartAsync(_hostCancellationTokenSource.Token);
    }

    public async Task StopQueue()
    {
        await queuedHostedService.StopAsync(_hostCancellationTokenSource.Token);
    }
    
    public async Task QueueItem()
    {
        await taskQueue.QueueBackgroundWorkItemAsync(BuildWorkItemAsync);
    }

    private async ValueTask BuildWorkItemAsync(CancellationToken token)
    {
        // Simulate three 5-second tasks to complete
        // for each enqueued work item

        int delayLoop = 0;
        var guid = Guid.NewGuid();

        logger.LogInformation("Queued work item {Guid} is starting.", guid);

        while (!token.IsCancellationRequested && delayLoop < 3)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(5), token);
            }
            catch (OperationCanceledException)
            {
                // Prevent throwing if the Delay is cancelled
            }

            ++ delayLoop;

            logger.LogInformation("Queued work item {Guid} is running. {DelayLoop}/3", guid, delayLoop);
        }

        if (delayLoop is 3)
        {
            logger.LogInformation("Queued Background Task {Guid} is complete.", guid);
        }
        else
        {
            logger.LogInformation("Queued Background Task {Guid} was cancelled.", guid);
        }
    }
}