using BackgroundServiceTestApp.Pages;
using BackgroundServiceTestApp.Queue;
using BackgroundServiceTestApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Shiny;

namespace BackgroundServiceTestApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseShiny()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        
        builder.Services.AddSingleton<IBackgroundTaskQueue>(_ => new DefaultBackgroundTaskQueue(100));
        builder.Services.AddSingleton<QueueManager>();
        builder.Services.AddSingleton<QueuedHostedService>();

        builder.Services.AddTransient<BackgroundTestPage>();
        builder.Services.AddTransient<BackgroundTestViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}