# BackgroundServiceTestApp

Test app for using .NET hosted services within maui

If you follow the [Create a Queue Service](https://learn.microsoft.com/en-us/dotnet/core/extensions/queue-service) tutorial you will use `BackgroundService` to implement a processing queue, this is a great piece of functionality however this approach isn't strictly compatible with Maui. The reason being is that Maui does not depend on `Microsoft.Extensions.Hosting`[(see #4393)](https://github.com/dotnet/maui/issues/4393) and has no way of actually executing the background services via the approach shown in the tutorial.

This proof of concept has adapted the sample for Maui to make use of the `BackgroundService` base class and execute background tasks from a client side app!

## Setup

- Install `Microsoft.Extensions.Hosting`
- Register your `BackgroundService` as a singleton, do not use `AddHostedService`:
```diff
- builder.Services.AddHostedService<QueuedHostedService>();
+ builder.Services.AddSingleton<QueuedHostedService>();
```
- Create a manager class for starting / stopping the queue, manually resolving the `BackgroundService` and calling `Start`/`Stop`
> You will need to handle cancellation manually, you cannot use IHostApplicationLifetime in Maui