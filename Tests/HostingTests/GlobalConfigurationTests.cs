using KageKirin.Extensions.Configuration.GitConfig;
using LibGit2Sharp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HostingTests;

[Collection("Sequential")]
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

        fixture.ConfigUserName(userName: userName, level: ConfigurationLevel.Global);
        fixture.ConfigUserEmail(userEmail: userEmail, level: ConfigurationLevel.Global);
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

[Collection("Sequential")]
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

        fixture.AddAlias(alias: aliasName, command: aliasCommand, level: ConfigurationLevel.Global);
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

[Collection("Sequential")]
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

        fixture.ConfigRebase(key: autostashKey, value: autostashValue, level: ConfigurationLevel.Global);
        fixture.ConfigRebase(key: autosquashKey, value: autosquashValue, level: ConfigurationLevel.Global);
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

[Collection("Sequential")]
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

        fixture.ConfigPull(key: rebaseKey, value: rebaseValue, level: ConfigurationLevel.Global);
        fixture.ConfigPull(key: autostashKey, value: autostashValue, level: ConfigurationLevel.Global);
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

[Collection("Sequential")]
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

        fixture.ToggleRerere(value: toggleValue, level: ConfigurationLevel.Global);
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

[Collection("Sequential")]
public class GlobalGearTokenConfigurationTest : IClassFixture<HostingFixture>
{
    private readonly HostingFixture fixture;
    private const string gearUrl = "gitlove.com";
    private readonly string gearToken = $"ghe_{Convert.ToBase64String(Guid.NewGuid().ToByteArray())}";

    public GlobalGearTokenConfigurationTest(HostingFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.AddGearsToken(url: gearUrl, token: gearToken, level: ConfigurationLevel.Global);
    }

    [Fact]
    public void TestLib()
    {
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        Assert.Equal(gearToken, fixture.Repository.Config.Get<string>($"gears.{gearUrl}.token").Value);
    }

    [Fact]
    public void TestConfig()
    {
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.RepoDirectory);
        Assert.NotEmpty(fixture.RepoDirectory);

        using IHost host = fixture.CreateHost();
        IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
        Assert.NotNull(config.GetSection("gears"));
        Assert.NotNull(config.GetRequiredSection("gears").GetChildren());
        Assert.NotNull(config.GetSection($"gears:{gearUrl.Replace(".", ":")}"));
        Assert.NotNull(config[$"gears:{gearUrl.Replace(".", ":")}:token"]);

        Assert.Equal(gearToken, config.GetRequiredSection("gears").GetRequiredSection(gearUrl.Replace(".", ":"))["token"]);
        Assert.Equal(gearToken, config[$"gears:{gearUrl.Replace(".", ":")}:token"]);
    }
}

[Collection("Sequential")]
public class GlobalLoggingLevelConfigurationTest : IClassFixture<HostingFixture>
{
    private readonly HostingFixture fixture;
    private const string loggingSection = "Default";
    private const string loggingLevel = "Trace";

    public GlobalLoggingLevelConfigurationTest(HostingFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.SetLogging(key: loggingSection, value: loggingLevel, level: ConfigurationLevel.Global);
    }

    [Fact]
    public void TestLib()
    {
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        Assert.Equal(loggingLevel, fixture.Repository.Config.Get<string>($"Logging.LogLevel.{loggingSection}").Value);
    }

    [Fact]
    public void TestConfig()
    {
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.RepoDirectory);
        Assert.NotEmpty(fixture.RepoDirectory);

        using IHost host = fixture.CreateHost();
        IConfiguration config = host.Services.GetRequiredService<IConfiguration>();

        Assert.Equal(loggingLevel, config.GetRequiredSection("Logging").GetRequiredSection("LogLevel")[loggingSection]);
    }
}
