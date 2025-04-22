using System;
using System.Diagnostics;
using LibGit2Sharp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace KageKirin.Extensions.Configuration.GitConfig;

public class GitConfigurationProvider : ConfigurationProvider, IDisposable
{
    private readonly IDisposable? changeTokenRegistration = default;
    private readonly IFileProvider? watchedFiles = default;

    readonly LibGit2Sharp.Configuration? configuration;
    readonly bool optional = true;

    public GitConfigurationProvider(LibGit2Sharp.Configuration configuration, bool optional = true)
    {
        this.configuration = configuration;
        this.optional = optional;
    }

    public GitConfigurationProvider(bool optional = true)
        : this(path: Environment.CurrentDirectory, optional: optional) { }

    public GitConfigurationProvider(string path, bool optional = true, bool reloadOnChange = false)
        : this(
            repositoryConfigurationPath: Repository.Discover(path),
            globalConfigurationPath: null,
            xdgConfigurationPath: null,
            systemConfigurationPath: null,
            optional: optional,
            reloadOnChange: reloadOnChange
        ) { }

    public GitConfigurationProvider(Repository repository, bool optional = true)
        : this(configuration: repository.Config, optional: optional) { }

    public GitConfigurationProvider(
        string repositoryConfigurationPath,
        string globalConfigurationPath,
        string xdgConfigurationPath,
        string systemConfigurationPath,
        bool optional = true,
        bool reloadOnChange = false
    )
        : this(
            configuration: LibGit2Sharp.Configuration.BuildFrom(
                repositoryConfigurationPath,
                globalConfigurationPath,
                xdgConfigurationPath,
                systemConfigurationPath
            ),
            optional: optional
        )
    {
        if (reloadOnChange)
        {
            List<IFileProvider> providers = new();
            if (!string.IsNullOrEmpty(repositoryConfigurationPath))
            {
                providers.Add(new PhysicalFileProvider(repositoryConfigurationPath));
            }
            if (!string.IsNullOrEmpty(globalConfigurationPath))
            {
                providers.Add(new PhysicalFileProvider(globalConfigurationPath));
            }
            if (!string.IsNullOrEmpty(xdgConfigurationPath))
            {
                providers.Add(new PhysicalFileProvider(xdgConfigurationPath));
            }
            if (!string.IsNullOrEmpty(systemConfigurationPath))
            {
                providers.Add(new PhysicalFileProvider(systemConfigurationPath));
            }
            watchedFiles = new CompositeFileProvider(providers);

            changeTokenRegistration = ChangeToken.OnChange(
                changeTokenProducer: () => watchedFiles.Watch("*.*"),
                changeTokenConsumer: () => Reload()
            );
        }
    }

    public override void Load()
    {
        if (!optional)
            Debug.Assert(configuration != null);

        if (configuration != null)
        {
            //configuration.ConfigurationEntry<T> Get<T>(string[] keyParts) for direct access? ms_configString.Split(':') to get key parts

            foreach (var entry in configuration)
            {
                Console.WriteLine($"[gitconfig] reading [{entry.Level}] {entry.Key}: {entry.Value}");
                Data[entry.Key.Replace(".", ":")] = entry.Value;
            }
        }
    }

    private void Reload()
    {
        Data.Clear();
        Load();
    }

    public void Dispose()
    {
        configuration?.Dispose();
    }
}
