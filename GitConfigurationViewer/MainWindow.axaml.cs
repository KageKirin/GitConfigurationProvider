using System;
using Avalonia.Controls;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GitConfigurationViewer;

public partial class MainWindow : Window
{
    public MainWindow(IConfiguration configuration, MainWindowViewModel viewModel, ILogger<MainWindow> logger)
    {
        logger.LogTrace("MainWindow.ctor");

        InitializeComponent();
        foreach (var config in configuration.GetSection("alias").GetChildren())
        {
            logger.LogTrace("{0}: {1}", config.Key, config.Value);
        }

        DataContext = viewModel;
    }
}
