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
}
