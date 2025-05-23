using KageKirin.Extensions.Configuration.GitConfig;
using LibGit2Sharp;
using Microsoft.Extensions.Configuration;

namespace ConfigurationTests;

[Collection("Sequential")]
public class GlobalUserConfigurationTest : IClassFixture<TestRepositoryFixture>
{
    private readonly TestRepositoryFixture fixture;
    private const string userName = "globalUser";
    private const string userEmail = "global@localhost.com";

    public GlobalUserConfigurationTest(TestRepositoryFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.Repository.Config.ConfigUserName(userName: userName);
        fixture.Repository.Config.ConfigUserEmail(userEmail: userEmail);
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

        IConfiguration config = new ConfigurationBuilder().AddGitConfig(path: fixture.RepoDirectory).Build();

        Assert.Equal(userName, config["user:name"]);
        Assert.Equal(userEmail, config["user:email"]);
    }
}

[Collection("Sequential")]
public class LocalUserConfigurationTest : IClassFixture<TestRepositoryFixture>
{
    private readonly TestRepositoryFixture fixture;
    private const string globalUserName = "globalUser";
    private const string globalUserEmail = "global@localhost.com";
    private const string localUserName = "localUser";
    private const string localUserEmail = "local@localhost.com";

    public LocalUserConfigurationTest(TestRepositoryFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.Repository.Config.ConfigUserName(userName: globalUserName);
        fixture.Repository.Config.ConfigUserEmail(userEmail: globalUserEmail);
        fixture.Repository.Config.ConfigLocalUserName(userName: localUserName);
        fixture.Repository.Config.ConfigLocalUserEmail(userEmail: localUserEmail);
    }

    [Fact]
    public void TestLib()
    {
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        Assert.Equal(localUserName, fixture.Repository.Config.Get<string>("user.name").Value);
        Assert.Equal(localUserEmail, fixture.Repository.Config.Get<string>("user.email").Value);
    }

    [Fact]
    public void TestConfig()
    {
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.RepoDirectory);
        Assert.NotEmpty(fixture.RepoDirectory);

        IConfiguration config = new ConfigurationBuilder().AddGitConfig(path: fixture.RepoDirectory).Build();

        Assert.Equal(localUserName, config["user:name"]);
        Assert.Equal(localUserEmail, config["user:email"]);
    }
}

[Collection("Sequential")]
public class GlobalAliasConfigurationTest : IClassFixture<TestRepositoryFixture>
{
    private readonly TestRepositoryFixture fixture;
    private const string aliasName = "foobar";
    private const string aliasCommand = "add --patch";

    public GlobalAliasConfigurationTest(TestRepositoryFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.Repository.Config.AddAlias(alias: aliasName, command: aliasCommand);
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

        IConfiguration config = new ConfigurationBuilder().AddGitConfig(path: fixture.RepoDirectory).Build();

        Assert.Equal(aliasCommand, config.GetRequiredSection("alias")[aliasName]);
    }
}

[Collection("Sequential")]
public class LocalAliasConfigurationTest : IClassFixture<TestRepositoryFixture>
{
    private readonly TestRepositoryFixture fixture;
    private const string aliasName = "foobar";
    private const string aliasCommand = "add --patch";

    public LocalAliasConfigurationTest(TestRepositoryFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.Repository.Config.AddLocalAlias(alias: aliasName, command: aliasCommand);
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

        IConfiguration config = new ConfigurationBuilder().AddGitConfig(path: fixture.RepoDirectory).Build();

        Assert.Equal(aliasCommand, config.GetRequiredSection("alias")[aliasName]);
    }
}

[Collection("Sequential")]
public class GlobalRebaseConfigurationTest : IClassFixture<TestRepositoryFixture>
{
    private readonly TestRepositoryFixture fixture;
    private const string autostashKey = "autostash";
    private const bool autostashValue = true;
    private const string autosquashKey = "autosquash";
    private const bool autosquashValue = true;

    public GlobalRebaseConfigurationTest(TestRepositoryFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.Repository.Config.ConfigRebase(key: autostashKey, value: autostashValue);
        fixture.Repository.Config.ConfigRebase(key: autosquashKey, value: autosquashValue);
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

        IConfiguration config = new ConfigurationBuilder().AddGitConfig(path: fixture.RepoDirectory).Build();

        Assert.Equal("true", config.GetRequiredSection("rebase")[autostashKey]);
        Assert.Equal("true", config.GetRequiredSection("rebase")[autosquashKey]);
    }
}

[Collection("Sequential")]
public class LocalRebaseConfigurationTest : IClassFixture<TestRepositoryFixture>
{
    private readonly TestRepositoryFixture fixture;
    private const string autostashKey = "autostash";
    private const bool autostashValue = true;
    private const string autosquashKey = "autosquash";
    private const bool autosquashValue = true;

    public LocalRebaseConfigurationTest(TestRepositoryFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.Repository.Config.ConfigLocalRebase(key: autostashKey, value: autostashValue);
        fixture.Repository.Config.ConfigLocalRebase(key: autosquashKey, value: autosquashValue);
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

        IConfiguration config = new ConfigurationBuilder().AddGitConfig(path: fixture.RepoDirectory).Build();

        Assert.Equal("true", config.GetRequiredSection("rebase")[autostashKey]);
        Assert.Equal("true", config.GetRequiredSection("rebase")[autosquashKey]);
    }
}

[Collection("Sequential")]
public class GlobalPullConfigurationTest : IClassFixture<TestRepositoryFixture>
{
    private readonly TestRepositoryFixture fixture;
    private const string rebaseKey = "rebase";
    private const bool rebaseValue = true;
    private const string autostashKey = "autostash";
    private const bool autostashValue = true;

    public GlobalPullConfigurationTest(TestRepositoryFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.Repository.Config.ConfigPull(key: rebaseKey, value: rebaseValue);
        fixture.Repository.Config.ConfigPull(key: autostashKey, value: autostashValue);
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

        IConfiguration config = new ConfigurationBuilder().AddGitConfig(path: fixture.RepoDirectory).Build();

        Assert.Equal("true", config.GetRequiredSection("pull")[rebaseKey]);
        Assert.Equal("true", config.GetRequiredSection("pull")[autostashKey]);
    }
}

[Collection("Sequential")]
public class LocalPullConfigurationTest : IClassFixture<TestRepositoryFixture>
{
    private readonly TestRepositoryFixture fixture;
    private const string rebaseKey = "rebase";
    private const bool rebaseValue = true;
    private const string autostashKey = "autostash";
    private const bool autostashValue = true;

    public LocalPullConfigurationTest(TestRepositoryFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.Repository.Config.ConfigLocalPull(key: rebaseKey, value: rebaseValue);
        fixture.Repository.Config.ConfigLocalPull(key: autostashKey, value: autostashValue);
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

        IConfiguration config = new ConfigurationBuilder().AddGitConfig(path: fixture.RepoDirectory).Build();

        Assert.Equal("true", config.GetRequiredSection("pull")[rebaseKey]);
        Assert.Equal("true", config.GetRequiredSection("pull")[autostashKey]);
    }
}

[Collection("Sequential")]
public class GlobalRerereConfigurationTest : IClassFixture<TestRepositoryFixture>
{
    private readonly TestRepositoryFixture fixture;

    private const bool toggleValue = true;

    public GlobalRerereConfigurationTest(TestRepositoryFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.Repository.Config.ToggleRerere(value: toggleValue);
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

        IConfiguration config = new ConfigurationBuilder().AddGitConfig(path: fixture.RepoDirectory).Build();

        Assert.Equal("true", config.GetRequiredSection("rerere")["enabled"]);
    }
}

[Collection("Sequential")]
public class LocalRerereConfigurationTest : IClassFixture<TestRepositoryFixture>
{
    private readonly TestRepositoryFixture fixture;

    private const bool toggleValue = true;

    public LocalRerereConfigurationTest(TestRepositoryFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.Repository.Config.ToggleLocalRerere(value: toggleValue);
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

        IConfiguration config = new ConfigurationBuilder().AddGitConfig(path: fixture.RepoDirectory).Build();

        Assert.Equal("true", config.GetRequiredSection("rerere")["enabled"]);
    }
}

[Collection("Sequential")]
public class GlobalGearTokenConfigurationTest : IClassFixture<TestRepositoryFixture>
{
    private readonly TestRepositoryFixture fixture;
    private const string gearUrl = "gitlove.com";
    private readonly string gearToken = $"ghe_{Convert.ToBase64String(Guid.NewGuid().ToByteArray())}";

    public GlobalGearTokenConfigurationTest(TestRepositoryFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.Repository.Config.AddGearsToken(url: gearUrl, token: gearToken);
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

        IConfiguration config = new ConfigurationBuilder().AddGitConfig(path: fixture.RepoDirectory).Build();
        Assert.NotNull(config.GetSection("gears"));
        Assert.NotNull(config.GetRequiredSection("gears").GetChildren());
        Assert.NotNull(config.GetSection($"gears:{gearUrl.Replace(".", ":")}"));
        Assert.NotNull(config[$"gears:{gearUrl.Replace(".", ":")}:token"]);

        Assert.Equal(gearToken, config.GetRequiredSection("gears").GetRequiredSection(gearUrl.Replace(".", ":"))["token"]);
        Assert.Equal(gearToken, config[$"gears:{gearUrl.Replace(".", ":")}:token"]);
    }
}

[Collection("Sequential")]
public class LocalGearTokenConfigurationTest : IClassFixture<TestRepositoryFixture>
{
    private readonly TestRepositoryFixture fixture;
    private const string gearUrl = "gitlove.com";
    private readonly string gearToken = $"ghe_{Convert.ToBase64String(Guid.NewGuid().ToByteArray())}";

    public LocalGearTokenConfigurationTest(TestRepositoryFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.Repository.Config.AddLocalGearsToken(url: gearUrl, token: gearToken);
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

        IConfiguration config = new ConfigurationBuilder().AddGitConfig(path: fixture.RepoDirectory).Build();
        Assert.NotNull(config.GetSection("gears"));
        Assert.NotNull(config.GetRequiredSection("gears").GetChildren());
        Assert.NotNull(config.GetSection($"gears:{gearUrl.Replace(".", ":")}"));
        Assert.NotNull(config[$"gears:{gearUrl.Replace(".", ":")}:token"]);

        Assert.Equal(gearToken, config.GetRequiredSection("gears").GetRequiredSection(gearUrl.Replace(".", ":"))["token"]);
        Assert.Equal(gearToken, config[$"gears:{gearUrl.Replace(".", ":")}:token"]);
    }
}

[Collection("Sequential")]
public class GlobalLoggingLevelConfigurationTest : IClassFixture<TestRepositoryFixture>
{
    private readonly TestRepositoryFixture fixture;
    private const string loggingSection = "Default";
    private const string loggingLevel = "Trace";

    public GlobalLoggingLevelConfigurationTest(TestRepositoryFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.Repository.Config.SetLogging(key: loggingSection, value: loggingLevel);
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

        IConfiguration config = new ConfigurationBuilder().AddGitConfig(path: fixture.RepoDirectory).Build();

        Assert.Equal(loggingLevel, config.GetRequiredSection("Logging").GetRequiredSection("LogLevel")[loggingSection]);
    }
}

[Collection("Sequential")]
public class LocalLoggingLevelConfigurationTest : IClassFixture<TestRepositoryFixture>
{
    private readonly TestRepositoryFixture fixture;
    private const string loggingSection = "Default";
    private const string loggingLevel = "Trace";

    public LocalLoggingLevelConfigurationTest(TestRepositoryFixture fixture)
    {
        this.fixture = fixture;
        Assert.NotNull(fixture);
        Assert.NotNull(fixture.Repository);
        Assert.NotNull(fixture.Repository.Config);

        fixture.Repository.Config.SetLocalLogging(key: loggingSection, value: loggingLevel);
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

        IConfiguration config = new ConfigurationBuilder().AddGitConfig(path: fixture.RepoDirectory).Build();

        Assert.Equal(loggingLevel, config.GetRequiredSection("Logging").GetRequiredSection("LogLevel")[loggingSection]);
    }
}
