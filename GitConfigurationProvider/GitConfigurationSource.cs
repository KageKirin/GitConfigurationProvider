using System;
using Microsoft.Extensions.Configuration;

namespace KageKirin.Extensions.Configuration.GitConfig;

public class GitConfigurationSource : IConfigurationSource
{
    readonly Func<GitConfigurationProvider> buildAction;

    public GitConfigurationSource()
    {
        buildAction = () => new GitConfigurationProvider();
    }

    public GitConfigurationSource(string path)
    {
        buildAction = () => new GitConfigurationProvider(path: path);
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return buildAction();
    }
}
