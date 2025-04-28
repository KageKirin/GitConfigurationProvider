using System;
using System.IO;
using Xunit;

namespace ConfigurationTests;

public class TempDirectory : IDisposable
{
    public readonly string RepoDirectory;

    public TempDirectory(string? prefix = default)
    {
        RepoDirectory = Path.GetFullPath(Directory.CreateTempSubdirectory(prefix).FullName);
        Assert.True(Directory.Exists(RepoDirectory));
    }

    ~TempDirectory()
    {
        Dispose(false);
    }

    public virtual void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        Directory.Delete(RepoDirectory, recursive: true);
    }
}

public class TempDirectoryFixture : TempDirectory
{
    public TempDirectoryFixture()
        : base(prefix: null) { }
}
