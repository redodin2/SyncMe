﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using Xamarin.Essentials;

namespace SyncMe;

public partial class App : Application, INotifyPropertyChanged
{
    private static IServiceProvider _serviceProvider;
    private readonly ILogger<App> _logger;

    public static object AuthUIParent { get; set; }

    private IDisposable _appScope;

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        _logger = serviceProvider.GetRequiredService<ILogger<App>>();

        VersionTracking.Track();
    }

    protected override void OnStart()
    {
        _logger.LogInformation(nameof(OnStart) + " called");

        if (_appScope is not null)
            CloseScope();

        _appScope = _serviceProvider.CreateScope();

        MainPage = _serviceProvider.GetRequiredService<AppShell>();
    }

    protected override void OnSleep()
    {
        _logger.LogInformation(nameof(OnSleep) + " called");

        CloseScope();
    }

    protected override void OnResume()
    {
        _logger.LogInformation(nameof(OnResume) + " called");

        if (_appScope is not null)
            CloseScope();

        _appScope = _serviceProvider.CreateScope();
    }

    private void CloseScope()
    {
        _appScope.Dispose();
        _appScope = null;
    }
}
