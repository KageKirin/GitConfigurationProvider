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
