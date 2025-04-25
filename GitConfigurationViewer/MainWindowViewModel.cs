using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace GitConfigurationViewer;

public class MainWindowViewModel : ViewModelBase, IActivatableViewModel
{
    private IConfiguration configuration;

    public MainWindowViewModel(IConfiguration configuration, ILogger<MainWindowViewModel> logger)
    {
        logger.LogDebug("[MainWindowViewModel.ctor]");
        this.configuration = configuration;

        Aliases = new();
        RefreshAliases = ReactiveCommand.CreateFromTask(async _ =>
        {
            Aliases.Clear();
            foreach (var item in configuration.GetSection("alias").GetChildren())
            {
                Aliases.Add(new(item.Key, item?.Value ?? string.Empty));
                logger.LogDebug("setting {0} | {1}", item?.Key, item?.Value);
            }
        });

        this.WhenActivated(disposable =>
        {
            RefreshAliases.Execute().Subscribe().DisposeWith(disposable);
        });

        RefreshAliases.Execute();
    }

    public ReactiveCommand<Unit, Unit> RefreshAliases { get; }

    public ObservableCollection<KeyValuePair<string, string>> Aliases { get; }

    #region IActivatableViewModel
    public ViewModelActivator Activator { get; } = new();
    #endregion
}
