using System;
using LibGit2Sharp;

namespace ConfigurationTests;

public static class ConfigurationExtensions
{
    public static void ConfigUserName(this Configuration config, string userName) => config.Set("user.name", userName);

    public static void ConfigUserEmail(this Configuration config, string userEmail) => config.Set("user.email", userEmail);

    public static void ConfigLocalUserName(this Configuration config, string userName) =>
        config.Set("user.name", userName, level: ConfigurationLevel.Local);

    public static void ConfigLocalUserEmail(this Configuration config, string userEmail) =>
        config.Set("user.email", userEmail, level: ConfigurationLevel.Local);

    public static void AddAlias(this Configuration config, string alias, string command) => config.Set($"alias.{alias}", command);

    public static void AddLocalAlias(this Configuration config, string alias, string command) =>
        config.Set($"alias.{alias}", command, level: ConfigurationLevel.Local);

    public static void ConfigRebase(this Configuration config, string key, bool value) => config.Set($"rebase.{key}", value);

    public static void ConfigLocalRebase(this Configuration config, string key, bool value) =>
        config.Set($"rebase.{key}", value, level: ConfigurationLevel.Local);

    public static void ConfigPull(this Configuration config, string key, bool value) => config.Set($"pull.{key}", value);

    public static void ConfigLocalPull(this Configuration config, string key, bool value) =>
        config.Set($"pull.{key}", value, level: ConfigurationLevel.Local);

    public static void ToggleRerere(this Configuration config, bool value) => config.Set("rerere.enabled", value);

    public static void ToggleLocalRerere(this Configuration config, bool value) =>
        config.Set("rerere.enabled", value, level: ConfigurationLevel.Local);

    public static void AddGearsToken(this Configuration config, string url, string token) => config.Set($"gears.{url}.token", token);

    public static void AddLocalGearsToken(this Configuration config, string url, string token) =>
        config.Set($"gears.{url}.token", token, level: ConfigurationLevel.Local);

    public static void SetLogging(this Configuration config, string key, string value) => config.Set($"Logging.LogLevel.{key}", value);

    public static void SetLocalLogging(this Configuration config, string key, string value) =>
        config.Set($"Logging.LogLevel.{key}", value, level: ConfigurationLevel.Local);
}
