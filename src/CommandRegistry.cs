public static class CommandRegistry
{
    private static readonly List<Command> _commands = [];

    public static readonly Command Clear = Command.Create("clear",
                                                          "Clears the console.",
                                                          CommandType.BuiltIn,
                                                          shell => CommandActions.ExecuteClear());

    public static readonly Command Exit = Command.Create("exit",
                                                         "Exits the shell.",
                                                         CommandType.BuiltIn,
                                                         shell => CommandActions.ExecuteExit());

    public static readonly Command Echo = Command.Create("echo",
                                                         "Prints the provided arguments to the console.",
                                                         CommandType.BuiltIn,
                                                         shell => CommandActions.ExecuteEcho(shell.Arguments));

    public static readonly Command Type = Command.Create("type",
                                                         "Prints the type of the provided command.",
                                                         CommandType.BuiltIn,
                                                         shell => CommandActions.ExecuteType(shell.Command));

    public static readonly Command Pwd = Command.Create("pwd",
                                                        "Prints the current working directory.",
                                                        CommandType.BuiltIn,
                                                        shell => CommandActions.ExecutePwd(shell));

    public static readonly Command Cd = Command.Create("cd",
                                                        "Changes the current working directory.",
                                                        CommandType.BuiltIn,
                                                        shell => CommandActions.ExecuteCd(shell, shell.Arguments.FirstOrDefault()));
    static CommandRegistry()
    {
        _commands.Add(Clear);
        _commands.Add(Exit);
        _commands.Add(Echo);
        _commands.Add(Type);
        _commands.Add(Pwd);
        _commands.Add(Cd);
    }

    public static Command? GetCommand(string name) => _commands.FirstOrDefault(c => c.Name == name);
    public static bool IsValidCommand(string name) => _commands.Any(c => c.Name == name);
}