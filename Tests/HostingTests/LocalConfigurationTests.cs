using KageKirin.Extensions.Configuration.GitConfig;
using LibGit2Sharp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HostingTests;

public class LocalUserConfigurationTest : IClassFixture<HostingFixture>
{
    private readonly HostingFixture fixture;
    private const string userName = "localUser";
    private const string userEmail = "local@localhost.com";

    public LocalUserConfigurationTest(HostingFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.ConfigUserName(userName: userName, level: ConfigurationLevel.Local);
        fixture.ConfigUserEmail(userEmail: userEmail, level: ConfigurationLevel.Local);
    }

    [Fact]
    public void TestLib()
    {
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        Assert.Equal(userName, fixture.Repository.Config.Get<string>("user.name").Value);
        Assert.Equal(userEmail, fixture.Repository.Config.Get<string>("user.email").Value);
    }

    [Fact]
    public void TestConfig()
    {
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.RepoDirectory);
        Assert.NotEmpty(fixture.RepoDirectory);

        using IHost host = fixture.CreateHost();
        IConfiguration config = host.Services.GetRequiredService<IConfiguration>();

        Assert.Equal(userName, config["user:name"]);
        Assert.Equal(userEmail, config["user:email"]);
    }
}

public class LocalAliasConfigurationTest : IClassFixture<HostingFixture>
{
    private readonly HostingFixture fixture;
    private const string aliasName = "foobar";
    private const string aliasCommand = "add --patch";

    public LocalAliasConfigurationTest(HostingFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.AddAlias(alias: aliasName, command: aliasCommand, level: ConfigurationLevel.Local);
    }

    [Fact]
    public void TestLib()
    {
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        Assert.Equal(aliasCommand, fixture.Repository.Config.Get<string>($"alias.{aliasName}").Value);
    }

    [Fact]
    public void TestConfig()
    {
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.RepoDirectory);
        Assert.NotEmpty(fixture.RepoDirectory);

        using IHost host = fixture.CreateHost();
        IConfiguration config = host.Services.GetRequiredService<IConfiguration>();

        Assert.Equal(aliasCommand, config.GetRequiredSection("alias")[aliasName]);
    }
}

public class LocalRebaseConfigurationTest : IClassFixture<HostingFixture>
{
    private readonly HostingFixture fixture;
    private const string autostashKey = "autostash";
    private const bool autostashValue = true;
    private const string autosquashKey = "autosquash";
    private const bool autosquashValue = true;

    public LocalRebaseConfigurationTest(HostingFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.ConfigRebase(key: autostashKey, value: autostashValue, level: ConfigurationLevel.Local);
        fixture.ConfigRebase(key: autosquashKey, value: autosquashValue, level: ConfigurationLevel.Local);
    }

    [Fact]
    public void TestLib()
    {
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        Assert.True(fixture.Repository.Config.Get<bool>($"rebase.{autostashKey}").Value);
        Assert.True(fixture.Repository.Config.Get<bool>($"rebase.{autosquashKey}").Value);
    }

    [Fact]
    public void TestConfig()
    {
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.RepoDirectory);
        Assert.NotEmpty(fixture.RepoDirectory);

        using IHost host = fixture.CreateHost();
        IConfiguration config = host.Services.GetRequiredService<IConfiguration>();

        Assert.Equal("true", config.GetRequiredSection("rebase")[autostashKey]);
        Assert.Equal("true", config.GetRequiredSection("rebase")[autosquashKey]);
    }
}

public class LocalPullConfigurationTest : IClassFixture<HostingFixture>
{
    private readonly HostingFixture fixture;
    private const string rebaseKey = "rebase";
    private const bool rebaseValue = true;
    private const string autostashKey = "autostash";
    private const bool autostashValue = true;

    public LocalPullConfigurationTest(HostingFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.ConfigPull(key: rebaseKey, value: rebaseValue, level: ConfigurationLevel.Local);
        fixture.ConfigPull(key: autostashKey, value: autostashValue, level: ConfigurationLevel.Local);
    }

    [Fact]
    public void TestLib()
    {
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        Assert.True(fixture.Repository.Config.Get<bool>($"pull.{rebaseKey}").Value);
        Assert.True(fixture.Repository.Config.Get<bool>($"pull.{autostashKey}").Value);
    }

    [Fact]
    public void TestConfig()
    {
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.RepoDirectory);
        Assert.NotEmpty(fixture.RepoDirectory);

        using IHost host = fixture.CreateHost();
        IConfiguration config = host.Services.GetRequiredService<IConfiguration>();

        Assert.Equal("true", config.GetRequiredSection("pull")[rebaseKey]);
        Assert.Equal("true", config.GetRequiredSection("pull")[autostashKey]);
    }
}
