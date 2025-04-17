using System;
using Microsoft.Extensions.Configuration;

namespace KageKirin.Extensions.Configuration.GitConfig;

public static class GitConfigurationProviderExtension
{
    public static IConfigurationBuilder AddGitConfig(
        this IConfigurationBuilder builder
    ) => AddGitConfig(builder, path: Environment.CurrentDirectory);

    public static IConfigurationBuilder AddGitConfig(
        this IConfigurationBuilder builder,
        string path
    )
    {
        builder.Add(new GitConfigurationSource(path: path));
        return builder;
    }
}
