public static class CommandActions
{
    public static void ExecuteClear() => Console.Clear();

    public static void ExecuteExit() => Environment.Exit(0);

    public static void ExecuteEcho(IEnumerable<string> args) => Console.WriteLine(string.Join(' ', args));

    public static void ExecuteType(string? cmdName)
    {
        if (string.IsNullOrWhiteSpace(cmdName))
        {
            return;
        }

        var cmd = CommandRegistry.GetCommand(cmdName);

        if (cmd == null)
        {
            var executablePath = Helpers.GetExecutableCommandPath(cmdName);

            if (executablePath != null)
            {
                Console.WriteLine($"{cmdName} is {executablePath}");
                return;
            }

            Console.WriteLine($"{cmdName}: not found");
            return;
        }

        if (cmd.Type == CommandType.BuiltIn)
        {
            Console.WriteLine($"{cmdName} is a shell builtin");
        }
    }

    public static void ExecutePwd(Shell shell) => Console.WriteLine(shell.CurrentDirectory);

    public static void ExecuteCd(Shell shell, string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return;
        }

        shell.CurrentDirectory = path;

    }
}