using KageKirin.Extensions.Configuration.GitConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.Sources.Clear();
builder.Configuration.AddGitConfig(path: Environment.CurrentDirectory, optional: false, reloadOnChange: true);

using IHost host = builder.Build();
await host.RunAsync();
