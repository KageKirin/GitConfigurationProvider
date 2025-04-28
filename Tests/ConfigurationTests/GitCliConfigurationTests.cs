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
