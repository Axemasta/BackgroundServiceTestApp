using System.Threading.Tasks;
using BackgroundServiceTestApp.Queue;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace BackgroundServiceTestApp.ViewModels;

public partial class BackgroundTestViewModel(ILogger<BackgroundTestViewModel> logger, QueueManager queueManager) : ObservableObject
{
    [RelayCommand]
    private async Task Start()
    {
        logger.LogInformation("Starting background service");

        queueManager.StartQueue();
    }
    
    [RelayCommand]
    private async Task Stop()
    {
        logger.LogInformation("Stopping background service");
        
        queueManager.StopQueue();
    }
    
    [RelayCommand]
    private async Task QueueItem()
    {
        logger.LogInformation("Queuing item");

        await queueManager.QueueItem();
    }
}