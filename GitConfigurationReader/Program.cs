using KageKirin.Extensions.Configuration.GitConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.Sources.Clear();
builder.Configuration.AddGitConfig();

builder.Services.AddTransient<LogConfigService>();
using IHost host = builder.Build();

var my = host.Services.GetRequiredService<LogConfigService>();
await my.ExecuteAsync();
