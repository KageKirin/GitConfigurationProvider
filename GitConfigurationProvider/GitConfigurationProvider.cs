using System;
using System.Diagnostics;
using LibGit2Sharp;
using Microsoft.Extensions.Configuration;

namespace KageKirin.Extensions.Configuration.GitConfig;

public class GitConfigurationProvider : ConfigurationProvider, IDisposable
{
    readonly LibGit2Sharp.Configuration? configuration;

    public GitConfigurationProvider()
        : this(Environment.CurrentDirectory) { }

    public GitConfigurationProvider(string path)
    {
        configuration = LibGit2Sharp.Configuration.BuildFrom(Repository.Discover(path), null, null, null);
    }

    public override void Load()
    {
        Debug.Assert(configuration != null);

        foreach (var entry in configuration)
        {
            Console.WriteLine($"[gitconfig] reading [{entry.Level}] {entry.Key}: {entry.Value}");
            Data[entry.Key.Replace(".", ":")] = entry.Value;
        }
    }

    public void Dispose()
    {
        configuration?.Dispose();
    }
}
