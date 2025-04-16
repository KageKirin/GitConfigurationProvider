# Git Configuration Provider

A `ConfigurationProvider` (Microsoft.Extensions.Configuration) for `.gitconfig` files,  
relying on [libGitSharp](https://github.com/libgit2/libgit2sharp) for the git part.

## 📦 NuGet

### `Directory.Packages.props`

```xml
<Project>
  <ItemGroup Label="configuration dependencies">
    <PackageVersion Include="Microsoft.Extensions.Configuration" Version="9.0.2" />
    <!-- ... (more deps) -->
    <PackageVersion Include="KageKirin.Extensions.Configuration.GitConfig" Version="1.0.0" />
  </ItemGroup>
</Project>
```

### `project.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <!-- package dependencies -->
  <ItemGroup Label="configuration dependencies">
    <PackageReference Include="Microsoft.Extensions.Configuration" />
    <!-- ... (more deps) -->
    <PackageReference Include="KageKirin.Extensions.Configuration.GitConfig" />
  </ItemGroup>
</Project>
```

## ⚙️ Usage

Somewhere in your application startup code:

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using KageKirin.Extensions.Configuration.GitConfig;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.Sources.Clear();
builder.Configuration
    .AddGitConfig(optional: true, reloadOnChange: true);

using IHost host = builder.Build();
await host.RunAsync();
```

## 🔧 Building and Running

### 🔨 Building

```bash
dotnet build
```

### ▶ Running the Unit tests

```bash
dotnet test
```

### ▶ Running the Test project

```bash
dotnet run --project GitConfigurationReader
```

## 🤝 Collaborate with My Project

Please refer to [COLLABORATION.md](COLLABORATION.md).
