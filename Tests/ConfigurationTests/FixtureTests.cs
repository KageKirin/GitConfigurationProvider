using Xunit;

namespace ConfigurationTests;

public class UsingTempDirectoryTest
{
    [Fact]
    public void Test()
    {
        Assert.True(true);

        string path = string.Empty;

        using (TempDirectory fixture = new("foobar"))
        {
            Console.WriteLine($"fixture.RepoDirectory: {fixture.RepoDirectory}");
            Assert.True(Directory.Exists(fixture.RepoDirectory));
            path = fixture.RepoDirectory;
        }

        Assert.False(Path.Exists(path));
    }
}

public class TempDirectoryFixtureTest : IClassFixture<TempDirectoryFixture>
{
    private readonly TempDirectoryFixture fixture;

    public TempDirectoryFixtureTest(TempDirectoryFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void Test()
    {
        Assert.True(true);
        Console.WriteLine($"fixture.RepoDirectory: {fixture.RepoDirectory}");
        Assert.True(Directory.Exists(fixture.RepoDirectory));
    }
}

public class TestRepositoryFixtureTest : IClassFixture<TestRepositoryFixture>
{
    private readonly TestRepositoryFixture fixture;

    public TestRepositoryFixtureTest(TestRepositoryFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void Test()
    {
        Assert.True(true);
        Console.WriteLine($"fixture.RepoDirectory: {fixture.RepoDirectory}");
        Assert.True(Directory.Exists(fixture.RepoDirectory));
    }
}

public class CliGitTestRepositoryFixtureTest : IClassFixture<CliGitTestRepositoryFixture>
{
    private readonly CliGitTestRepositoryFixture fixture;

    public CliGitTestRepositoryFixtureTest(CliGitTestRepositoryFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void Test()
    {
        Assert.True(true);
        Console.WriteLine($"fixture.RepoDirectory: {fixture.RepoDirectory}");
        Assert.True(Directory.Exists(fixture.RepoDirectory));
    }
}
