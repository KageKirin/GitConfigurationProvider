using System;
using LibGit2Sharp;
using Microsoft.Extensions.Configuration;

namespace KageKirin.Extensions.Configuration.GitConfig;

public static class GitConfigurationProviderExtension
{
    public static IConfigurationBuilder AddGitConfig(this IConfigurationBuilder builder, bool optional = true) =>
        AddGitConfig(builder, path: Environment.CurrentDirectory, optional: optional);

    public static IConfigurationBuilder AddGitConfig(
        this IConfigurationBuilder builder,
        string path,
        bool optional = true,
        bool reloadOnChange = false
    )
    {
        builder.Add(new GitConfigurationSource(path: path, optional: optional, reloadOnChange: reloadOnChange));
        return builder;
    }

    public static IConfigurationBuilder AddGitConfig(this IConfigurationBuilder builder, Repository repository, bool optional = true)
    {
        builder.Add(new GitConfigurationSource(repository, optional: optional));
        return builder;
    }

    public static IConfigurationBuilder AddGitConfig(
        this IConfigurationBuilder builder,
        string repositoryConfigurationPath,
        string globalConfigurationPath,
        string xdgConfigurationPath,
        string systemConfigurationPath,
        bool optional = true,
        bool reloadOnChange = false
    )
    {
        builder.Add(
            new GitConfigurationSource(
                repositoryConfigurationPath: repositoryConfigurationPath,
                globalConfigurationPath: globalConfigurationPath,
                xdgConfigurationPath: xdgConfigurationPath,
                systemConfigurationPath: systemConfigurationPath,
                optional: optional,
                reloadOnChange: reloadOnChange
            )
        );
        return builder;
    }
}
