using System;
using System.Diagnostics;
using System.Text;

namespace ConfigurationTests;

public static class CliGit
{
    internal static int RunCommand(string command) => RunCommand(command.Split(' '));

    internal static int RunCommand(string[] command)
    {
        ProcessStartInfo startInfo =
            new(fileName: command[0], arguments: string.Join(" ", command[1..]))
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };
        StringBuilder stdout = new();
        StringBuilder stderr = new();

        using Process process = Process.Start(startInfo)!;
        process.OutputDataReceived += (sender, args) => stdout.AppendLine(args.Data);
        process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
        process.ErrorDataReceived += (sender, args) => stderr.AppendLine(args.Data);
        process.ErrorDataReceived += (sender, args) => Console.Error.WriteLine(args.Data);
        process.WaitForExit();

        return process.ExitCode;
    }

    public static int Init(string path) => RunCommand($"git -C {path} init");

    public static int ConfigUserName(string path, string userName) => RunCommand($"git -C {path} config user.name {userName}");

    public static int ConfigUserEmail(string path, string userEmail) => RunCommand($"git -C {path} config user.email {userEmail}");

    public static int ConfigLocalUserName(string path, string userName) => RunCommand($"git -C {path} config --local user.name {userName}");

    public static int ConfigLocalUserEmail(string path, string userEmail) =>
        RunCommand($"git -C {path} config --local user.email {userEmail}");

    public static int AddAlias(string path, string alias, string command) =>
        RunCommand($"git -C {path} config alias.{alias} \"{command}\"");

    public static int AddLocalAlias(string path, string alias, string command) =>
        RunCommand($"git -C {path} config --local alias.{alias} \"{command}\"");

    public static int ConfigRebase(string path, string key, bool value) =>
        RunCommand($"git -C {path} config rebase.{key} {value.ToString().ToLowerInvariant()}");

    public static int ConfigLocalRebase(string path, string key, bool value) =>
        RunCommand($"git -C {path} config --local rebase.{key} {value.ToString().ToLowerInvariant()}");

    public static int ConfigPull(string path, string key, bool value) =>
        RunCommand($"git -C {path} config pull.{key} {value.ToString().ToLowerInvariant()}");

    public static int ConfigLocalPull(string path, string key, bool value) =>
        RunCommand($"git -C {path} config --local pull.{key} {value.ToString().ToLowerInvariant()}");

    public static int ToggleRerere(string path, bool value) =>
        RunCommand($"git -C {path} config rerere.enabled {value.ToString().ToLowerInvariant()}");

    public static int ToggleLocalRerere(string path, bool value) =>
        RunCommand($"git -C {path} config --local rerere.enabled {value.ToString().ToLowerInvariant()}");

    public static int AddGearsToken(string path, string url, string token) =>
        RunCommand($"git -C {path} config gears.\"{url}\".token {token}");

    public static int AddLocalGearsToken(string path, string url, string token) =>
        RunCommand($"git -C {path} config --local gears.\"{url}\".token {token}");

    public static int SetLogging(string path, string key, string value) =>
        RunCommand($"git -C {path} config Logging.LogLevel.\"{key}\" {value}");

    public static int SetLocalLogging(string path, string key, string value) =>
        RunCommand($"git -C {path} config --local Logging.LogLevel.\"{key}\" {value}");
}
