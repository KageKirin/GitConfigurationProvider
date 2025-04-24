using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using KageKirin.Extensions.Configuration.GitConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GitConfigurationViewer;

public partial class App : Application
{
    public IHost? GlobalHost { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        var hostBuilder = CreateHostBuilder();
        GlobalHost = hostBuilder.Build();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = GlobalHost.Services.GetRequiredService<MainWindow>();
            desktop.Exit += (sender, args) =>
            {
                GlobalHost.StopAsync(TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();
                GlobalHost.Dispose();
                GlobalHost = null;
            };
        }

        base.OnFrameworkInitializationCompleted();

        // start Host as background task to avoid blocking the main UI thread
        await GlobalHost.StartAsync();
    }

    private static HostApplicationBuilder CreateHostBuilder()
    {
        // alternative: use Host.CreateDefaultBuilder
        var builder = Host.CreateApplicationBuilder(Environment.GetCommandLineArgs());
        builder.Configuration.Sources.Clear();
        builder.Configuration.AddGitConfig(path: Environment.CurrentDirectory, reloadOnChange: true);
        //builder.Services.AddTransient<LogConfigService>();

        builder.Services.AddTransient<MainWindow>();

        return builder;
    }
}
