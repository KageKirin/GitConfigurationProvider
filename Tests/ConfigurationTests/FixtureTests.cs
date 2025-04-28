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
