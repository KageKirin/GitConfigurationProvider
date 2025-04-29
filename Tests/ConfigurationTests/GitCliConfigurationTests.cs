using KageKirin.Extensions.Configuration.GitConfig;
using LibGit2Sharp;
using Microsoft.Extensions.Configuration;

namespace ConfigurationTests;

public class CliGitGlobalUserConfigurationTest : IClassFixture<CliGitTestRepositoryFixture>
{
    private readonly CliGitTestRepositoryFixture fixture;
    private const string userName = "globalUser";
    private const string userEmail = "global@localhost.com";

    public CliGitGlobalUserConfigurationTest(CliGitTestRepositoryFixture fixture)
    {
        this.fixture = fixture;

        CliGit.ConfigUserName(path: fixture.RepoDirectory, userName: userName);
        CliGit.ConfigUserEmail(path: fixture.RepoDirectory, userEmail: userEmail);
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

public class CliGitLocalUserConfigurationTest : IClassFixture<CliGitTestRepositoryFixture>
{
    private readonly CliGitTestRepositoryFixture fixture;
    private const string globalUserName = "globalUser";
    private const string globalUserEmail = "global@localhost.com";
    private const string localUserName = "localUser";
    private const string localUserEmail = "local@localhost.com";

    public CliGitLocalUserConfigurationTest(CliGitTestRepositoryFixture fixture)
    {
        this.fixture = fixture;

        CliGit.ConfigUserName(path: fixture.RepoDirectory, userName: globalUserName);
        CliGit.ConfigUserEmail(path: fixture.RepoDirectory, userEmail: globalUserEmail);
        CliGit.ConfigLocalUserName(path: fixture.RepoDirectory, userName: localUserName);
        CliGit.ConfigLocalUserEmail(path: fixture.RepoDirectory, userEmail: localUserEmail);
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

public class CliGitGlobalAliasConfigurationTest : IClassFixture<CliGitTestRepositoryFixture>
{
    private readonly CliGitTestRepositoryFixture fixture;
    private const string aliasName = "foobar";
    private const string aliasCommand = "add --patch";

    public CliGitGlobalAliasConfigurationTest(CliGitTestRepositoryFixture fixture)
    {
        this.fixture = fixture;

        CliGit.AddAlias(path: fixture.RepoDirectory, alias: aliasName, command: aliasCommand);
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

public class CliGitLocalAliasConfigurationTest : IClassFixture<CliGitTestRepositoryFixture>
{
    private readonly CliGitTestRepositoryFixture fixture;
    private const string aliasName = "foobar";
    private const string aliasCommand = "add --patch";

    public CliGitLocalAliasConfigurationTest(CliGitTestRepositoryFixture fixture)
    {
        this.fixture = fixture;

        CliGit.AddLocalAlias(path: fixture.RepoDirectory, alias: aliasName, command: aliasCommand);
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

public class CliGitGlobalRebaseConfigurationTest : IClassFixture<CliGitTestRepositoryFixture>
{
    private readonly CliGitTestRepositoryFixture fixture;
    private const string autostashKey = "autostash";
    private const bool autostashValue = true;
    private const string autosquashKey = "autosquash";
    private const bool autosquashValue = true;

    public CliGitGlobalRebaseConfigurationTest(CliGitTestRepositoryFixture fixture)
    {
        this.fixture = fixture;

        CliGit.ConfigRebase(path: fixture.RepoDirectory, key: autostashKey, value: autostashValue);
        CliGit.ConfigRebase(path: fixture.RepoDirectory, key: autosquashKey, value: autosquashValue);
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

public class CliGitLocalRebaseConfigurationTest : IClassFixture<CliGitTestRepositoryFixture>
{
    private readonly CliGitTestRepositoryFixture fixture;
    private const string autostashKey = "autostash";
    private const bool autostashValue = true;
    private const string autosquashKey = "autosquash";
    private const bool autosquashValue = true;

    public CliGitLocalRebaseConfigurationTest(CliGitTestRepositoryFixture fixture)
    {
        this.fixture = fixture;

        CliGit.ConfigLocalRebase(path: fixture.RepoDirectory, key: autostashKey, value: autostashValue);
        CliGit.ConfigLocalRebase(path: fixture.RepoDirectory, key: autosquashKey, value: autosquashValue);
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

public class CliGitGlobalPullConfigurationTest : IClassFixture<CliGitTestRepositoryFixture>
{
    private readonly CliGitTestRepositoryFixture fixture;
    private const string rebaseKey = "rebase";
    private const bool rebaseValue = true;
    private const string autostashKey = "autostash";
    private const bool autostashValue = true;

    public CliGitGlobalPullConfigurationTest(CliGitTestRepositoryFixture fixture)
    {
        this.fixture = fixture;

        CliGit.ConfigPull(path: fixture.RepoDirectory, key: rebaseKey, value: rebaseValue);
        CliGit.ConfigPull(path: fixture.RepoDirectory, key: autostashKey, value: autostashValue);
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

public class CliGitLocalPullConfigurationTest : IClassFixture<CliGitTestRepositoryFixture>
{
    private readonly CliGitTestRepositoryFixture fixture;
    private const string rebaseKey = "rebase";
    private const bool rebaseValue = true;
    private const string autostashKey = "autostash";
    private const bool autostashValue = true;

    public CliGitLocalPullConfigurationTest(CliGitTestRepositoryFixture fixture)
    {
        this.fixture = fixture;

        CliGit.ConfigLocalPull(path: fixture.RepoDirectory, key: rebaseKey, value: rebaseValue);
        CliGit.ConfigLocalPull(path: fixture.RepoDirectory, key: autostashKey, value: autostashValue);
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
