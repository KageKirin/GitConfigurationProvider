using System;
using System.IO;
using KageKirin.Extensions.Configuration.GitConfig;
using LibGit2Sharp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit;

namespace HostingTests;

public class HostingFixture : IDisposable
{
    public readonly string RepoDirectory;
    public readonly Repository Repository;

    public HostingFixture()
    {
        RepoDirectory = Directory.CreateTempSubdirectory().FullName;
        Assert.True(Directory.Exists(RepoDirectory));

        Repository.Init(path: RepoDirectory);
        Repository = new Repository(RepoDirectory);
        Assert.True(Repository != null);
        Assert.NotNull(Repository);
    }

    ~HostingFixture()
    {
        Dispose(false);
    }

    public virtual void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        Repository.Dispose();
        Directory.Delete(RepoDirectory, recursive: true);
    }

    public IHost CreateHost()
    {
        return Host.CreateDefaultBuilder()
            //< below: configure where the Host's configuration comes from
            .ConfigureHostConfiguration(builder => builder.AddGitConfig(path: RepoDirectory))
            //< below: configure how the Host should handle logging
            .ConfigureLogging(
                (context, builder) =>
                    builder
                        .AddConfiguration(context.Configuration.GetSection("logging"))
                        .AddConsole() //< add console as logging target
                        .AddDebug() //< add debug output as logging target
                        .SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace) //< set minimum level to trace in Debug
            )
            //< below: register services to be available in Host
            //.ConfigureServices(builder => builder.AddTransient<>())
            //< below: final step, create the host
            .Build();
    }

    public void ConfigUserName(string userName, ConfigurationLevel level = ConfigurationLevel.Local) =>
        Repository.Config.Set("user.name", userName);

    public void ConfigUserEmail(string userEmail, ConfigurationLevel level = ConfigurationLevel.Local) =>
        Repository.Config.Set("user.email", userEmail);

    public void AddAlias(string alias, string command, ConfigurationLevel level = ConfigurationLevel.Local) =>
        Repository.Config.Set($"alias.{alias}", command);

    public void ConfigRebase(string key, bool value, ConfigurationLevel level = ConfigurationLevel.Local) =>
        Repository.Config.Set($"rebase.{key}", value);

    public void ConfigPull(string key, bool value, ConfigurationLevel level = ConfigurationLevel.Local) =>
        Repository.Config.Set($"pull.{key}", value);

    public void ToggleRerere(bool value, ConfigurationLevel level = ConfigurationLevel.Local) =>
        Repository.Config.Set("rerere.enabled", value);

    public void AddGearsToken(string url, string token, ConfigurationLevel level = ConfigurationLevel.Local) =>
        Repository.Config.Set($"gears.{url}.token", token);

    public void SetLogging(string key, string value, ConfigurationLevel level = ConfigurationLevel.Local) =>
        Repository.Config.Set($"Logging.LogLevel.{key}", value);
}
