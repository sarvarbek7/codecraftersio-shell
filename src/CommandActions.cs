using System.Security.AccessControl;
using Microsoft.VisualBasic;

public static class CommandActions
{
    public static void ExecuteClear() => Console.Clear();

    public static void ExecuteExit() => Environment.Exit(0);

    public static void ExecuteEcho(IEnumerable<string> args) => Console.WriteLine(string.Join(' ', args));

    public static void ExecuteType(IEnumerable<string> args)
    {
        var cmdName = args.FirstOrDefault();

        if (cmdName == null)
        {
            Console.WriteLine("type: missing argument");
            return;
        }

        var cmd = Commands.GetCommand(cmdName);

        if (cmd == null)
        {
            var path = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;

            var entries = path.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
                .Where(Directory.Exists)
                .Distinct();

            foreach (var entry in entries)
            {
                var files = Directory.GetFiles(entry, cmdName);

                foreach (var filePath in files)
                {
                    if (Path.GetFileName(filePath) == cmdName)
                    {
                        if (Environment.OSVersion.Platform == PlatformID.Unix)
                        {
#pragma warning disable CA1416 // Validate platform compatibility
                            if (IsExecutableUnix(filePath))
                            {
                                Console.WriteLine($"{cmdName} is {filePath}");
                                return;
                            }
                        }
                        else
                        {
                            if (IsExecutableWindows(filePath))
                            {
                                Console.WriteLine($"{cmdName} is {filePath}");
                                return;
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"{cmdName}: not found");
            return;
        }

        if (cmd.Type == CommandType.BuiltIn)
        {
            Console.WriteLine($"{cmdName} is a shell builtin");
        }
    }

    private static bool IsExecutableUnix(string path)
    {
        if (!File.Exists(path))
            return false;

        var mode = File.GetUnixFileMode(path);

        return mode.HasFlag(UnixFileMode.UserExecute) ||
               mode.HasFlag(UnixFileMode.GroupExecute) ||
               mode.HasFlag(UnixFileMode.OtherExecute);
    }

    private static bool IsExecutableWindows(string path)
    {
        if (!File.Exists(path))
            return false;

        var executableExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        ".exe", ".bat", ".cmd", ".com", ".ps1"
    };

        return executableExtensions.Contains(Path.GetExtension(path));
    }
}