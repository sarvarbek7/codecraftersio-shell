public class Command
{
    public string Name { get; }
    public string Description { get; }
    public CommandType Type { get; }
    public Action<IEnumerable<string>> Action { get; set; }

    private Command(string name,
                    string description,
                    CommandType type,
                    Action<IEnumerable<string>>? action = null)
    {
        Name = name;
        Description = description;
        Type = type;
        Action = action ?? (args => { });
    }

    public static Command Create(string name,
                                 string description,
                                 CommandType type,
                                 Action<IEnumerable<string>>? action) => new(name, description, type, action);
    public override string ToString() => $"{Name}: {Description} ({Type})";

    public override bool Equals(object? obj)
    {
        if (obj is Command other)
        {
            return Name == other.Name;
        }
        return false;
    }

    public override int GetHashCode() => Name.GetHashCode();
}

public static class Commands
{
    private static readonly List<Command> _commands = [];

    public static readonly Command Clear = Command.Create("clear",
                                                          "Clears the console.",
                                                          CommandType.BuiltIn,
                                                          args => CommandActions.ExecuteClear());
    
    public static readonly Command Exit = Command.Create("exit",
                                                         "Exits the shell.",
                                                         CommandType.BuiltIn,
                                                         args => CommandActions.ExecuteExit());

    public static readonly Command Echo = Command.Create("echo",
                                                         "Prints the provided arguments to the console.",
                                                         CommandType.BuiltIn,
                                                         args => CommandActions.ExecuteEcho(args));
    
    public static readonly Command Type = Command.Create("type",
                                                         "Prints the type of the provided command.",
                                                         CommandType.BuiltIn,
                                                         args => CommandActions.ExecuteType(args));

    static Commands()
    {
        _commands.Add(Clear);
        _commands.Add(Exit);
        _commands.Add(Echo);
        _commands.Add(Type);
    }

    public static Command? GetCommand(string name) => _commands.FirstOrDefault(c => c.Name == name);
    public static bool IsValidCommand(string name) => _commands.Any(c => c.Name == name);
}