using System.Diagnostics;

public class Shell
{
    private string _homeAlias = "~";

    private string _currentDirectory = Environment.CurrentDirectory;
    private string _currentCommand = string.Empty;
    private IEnumerable<string> _currentArguments = [];

    public string Command => _currentCommand;
    public IEnumerable<string> Arguments => _currentArguments;
    public string CurrentDirectory
    {
        get => _currentDirectory;
        set
        {
            if (value.StartsWith(_homeAlias))
            {
                value = value.Replace(_homeAlias, Environment.GetEnvironmentVariable("HOME") ?? string.Empty);
            }

            bool isAbsolutePath = Path.IsPathRooted(value);
            string newPath = isAbsolutePath
                ? value
                : Path.GetFullPath(Path.Combine(_currentDirectory, value));

            if (Directory.Exists(newPath))
            {
                if (newPath.Length > 1 && newPath.EndsWith(Path.DirectorySeparatorChar))
                {
                    newPath = newPath.TrimEnd(Path.DirectorySeparatorChar);
                }

                _currentDirectory = newPath;
            }
            else
            {
                Console.WriteLine($"cd: {value}: No such file or directory");
            }
        }
    }

    private void ExtractCommand(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            _currentCommand = string.Empty;
            _currentArguments = [];

            return;
        }

        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        _currentCommand = parts[0];
        _currentArguments = [.. parts.Skip(1)];
    }

    public void Run()
    {
        while (true)
        {
            Console.Write("$ ");

            var input = Console.ReadLine() ?? string.Empty;

            ExtractCommand(input);

            var command = CommandRegistry.GetCommand(_currentCommand);

            if (command == null)
            {
                if (Helpers.GetExecutableCommandPath(_currentCommand) != null)
                {
                    var process = Process.Start(new ProcessStartInfo
                    {
                        FileName = _currentCommand,
                        Arguments = string.Join(' ', _currentArguments),
                        UseShellExecute = false,
                        RedirectStandardError = false,
                        RedirectStandardOutput = false,
                    });

                    process?.WaitForExit();
                }
                else
                {
                    Console.WriteLine($"{_currentCommand}: command not found");
                }

                continue;
            }

            command.Action(this);
        }
    }
}