using KageKirin.Extensions.Configuration.GitConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HostingTests;

public class GlobalUserConfigurationTest : IClassFixture<HostingFixture>
{
    private readonly HostingFixture fixture;
    private const string userName = "globalUser";
    private const string userEmail = "global@localhost.com";

    public GlobalUserConfigurationTest(HostingFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.ConfigUserName(userName: userName);
        fixture.ConfigUserEmail(userEmail: userEmail);
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

public class GlobalAliasConfigurationTest : IClassFixture<HostingFixture>
{
    private readonly HostingFixture fixture;
    private const string aliasName = "foobar";
    private const string aliasCommand = "add --patch";

    public GlobalAliasConfigurationTest(HostingFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.AddAlias(alias: aliasName, command: aliasCommand);
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

public class GlobalRebaseConfigurationTest : IClassFixture<HostingFixture>
{
    private readonly HostingFixture fixture;
    private const string autostashKey = "autostash";
    private const bool autostashValue = true;
    private const string autosquashKey = "autosquash";
    private const bool autosquashValue = true;

    public GlobalRebaseConfigurationTest(HostingFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.ConfigRebase(key: autostashKey, value: autostashValue);
        fixture.ConfigRebase(key: autosquashKey, value: autosquashValue);
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

public class GlobalPullConfigurationTest : IClassFixture<HostingFixture>
{
    private readonly HostingFixture fixture;
    private const string rebaseKey = "rebase";
    private const bool rebaseValue = true;
    private const string autostashKey = "autostash";
    private const bool autostashValue = true;

    public GlobalPullConfigurationTest(HostingFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.ConfigPull(key: rebaseKey, value: rebaseValue);
        fixture.ConfigPull(key: autostashKey, value: autostashValue);
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

public class GlobalRerereConfigurationTest : IClassFixture<HostingFixture>
{
    private readonly HostingFixture fixture;

    private const bool toggleValue = true;

    public GlobalRerereConfigurationTest(HostingFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.ToggleRerere(value: toggleValue);
    }

    [Fact]
    public void TestLib()
    {
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        Assert.True(fixture.Repository.Config.Get<bool>($"rerere.enabled").Value);
    }

    [Fact]
    public void TestConfig()
    {
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.RepoDirectory);
        Assert.NotEmpty(fixture.RepoDirectory);

        using IHost host = fixture.CreateHost();
        IConfiguration config = host.Services.GetRequiredService<IConfiguration>();

        Assert.Equal("true", config.GetRequiredSection("rerere")["enabled"]);
    }
}
