public static class Helpers
{
    public static string? GetExecutableCommandPath(string cmdName)
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
                            return filePath;
                        }
                    }
                    else
                    {
                        if (IsExecutableWindows(filePath))
                        {
                            return filePath;
                        }
                    }
                }
            }
        }

        return null;
    }

    private static bool IsExecutableUnix(string path)
    {
        if (!File.Exists(path))
            return false;

#pragma warning disable CA1416 // Validate platform compatibility
        var mode = File.GetUnixFileMode(path);
#pragma warning restore CA1416 // Validate platform compatibility

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