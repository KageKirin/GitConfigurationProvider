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

    public override void OnFrameworkInitializationCompleted()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.Configuration.Sources.Clear();
        builder.Configuration.AddGitConfig(path: Environment.CurrentDirectory, reloadOnChange: true);
        //builder.Services.AddTransient<LogConfigService>();
        GlobalHost = builder.Build();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();

            desktop.Exit += (sender, args) =>
            {
                GlobalHost.StopAsync(TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();
                GlobalHost.Dispose();
                GlobalHost = null;
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
