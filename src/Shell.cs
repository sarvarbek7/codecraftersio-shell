using System.Diagnostics;

public class Shell
{
    private string _homeAlias = "~";

    private string _currentDirectory = Environment.CurrentDirectory;
    private string _currentCommand = string.Empty;
    private List<string> _currentArguments = [];

    public string Command => _currentCommand;
    public IReadOnlyList<string> Arguments => _currentArguments;
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

        var parser = new Parser();
        parser.TokenizeV2(input);

        _currentCommand = parser.Tokens.FirstOrDefault() ?? string.Empty;
        _currentArguments = [.. parser.Tokens.Skip(1)];
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
                    var processInfo = new ProcessStartInfo(_currentCommand, Arguments)
                    {
                        UseShellExecute = false,
                        RedirectStandardError = false,
                        RedirectStandardOutput = false
                    };

                    var process = Process.Start(processInfo);

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