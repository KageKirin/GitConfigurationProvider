using System;
using Microsoft.Extensions.Configuration;

namespace KageKirin.Extensions.Configuration.GitConfig;

public class GitConfigurationSource : IConfigurationSource
{
    readonly Func<GitConfigurationProvider> buildAction;

    public GitConfigurationSource(bool optional = true)
    {
        buildAction = () => new GitConfigurationProvider(optional: optional);
    }

    public GitConfigurationSource(string path, bool optional = true)
    {
        buildAction = () => new GitConfigurationProvider(path: path, optional: optional);
    }

    public GitConfigurationSource(
        string repositoryConfigurationPath,
        string globalConfigurationPath,
        string xdgConfigurationPath,
        string systemConfigurationPath,
        bool optional = true
    )
    {
        buildAction = () =>
            new GitConfigurationProvider(
                repositoryConfigurationPath: repositoryConfigurationPath,
                globalConfigurationPath: globalConfigurationPath,
                xdgConfigurationPath: xdgConfigurationPath,
                systemConfigurationPath: systemConfigurationPath,
                optional: optional
            );
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return buildAction();
    }
}
