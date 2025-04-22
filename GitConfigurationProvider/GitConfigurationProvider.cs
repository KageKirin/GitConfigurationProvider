using System;
using System.Diagnostics;
using LibGit2Sharp;
using Microsoft.Extensions.Configuration;

namespace KageKirin.Extensions.Configuration.GitConfig;

public class GitConfigurationProvider : ConfigurationProvider, IDisposable
{
    readonly LibGit2Sharp.Configuration? configuration;
    readonly bool optional = true;

    public GitConfigurationProvider(LibGit2Sharp.Configuration configuration, bool optional = true)
    {
        this.configuration = configuration;
        this.optional = optional;
    }

    public GitConfigurationProvider(bool optional = true)
        : this(path: Environment.CurrentDirectory, optional: optional) { }

    public GitConfigurationProvider(string path, bool optional = true)
        : this(LibGit2Sharp.Configuration.BuildFrom(Repository.Discover(path), null, null, null), optional: optional) { }

    public GitConfigurationProvider(
        string repositoryConfigurationPath,
        string globalConfigurationPath,
        string xdgConfigurationPath,
        string systemConfigurationPath,
        bool optional = true
    )
        : this(
            configuration: LibGit2Sharp.Configuration.BuildFrom(
                repositoryConfigurationPath,
                globalConfigurationPath,
                xdgConfigurationPath,
                systemConfigurationPath
            ),
            optional: optional
        ) { }

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

    public void Dispose()
    {
        configuration?.Dispose();
    }
}
