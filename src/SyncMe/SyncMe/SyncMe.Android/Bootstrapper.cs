﻿using Microsoft.Extensions.DependencyInjection;
using SyncMe.Droid.Alarm;
using SyncMe.Extensions;

namespace SyncMe.Droid;

public static class Bootstrapper
{
    private static IServiceProvider _instance;
    public static IServiceProvider Instance => _instance ??= CreateServiceProvider();
    public static T GetService<T>() => Instance.GetRequiredService<T>();

    private static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection()
          .AddSyncMeLib()
          .AddSyncMeAndroid();

        return DIDataTemplate.AppServiceProvider = services.BuildServiceProvider();
    }

    public static App CreateApp()
    {
        var app = new App(Instance);

        return app;
    }

    public static IServiceCollection AddSyncMeAndroid(this IServiceCollection services)
    {
        services
            .AddSingleton<IAndroidAlarmService, AndroidAlarmService>();

        return services;
    }
}
