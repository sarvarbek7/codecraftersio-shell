public class Command
{
    public string Name { get; }
    public string Description { get; }
    public CommandType Type { get; }
    public Action<Shell> Action { get; set; }

    private Command(string name,
                    string description,
                    CommandType type,
                    Action<Shell>? action = null)
    {
        Name = name;
        Description = description;
        Type = type;
        Action = action ?? (shell => { });
    }

    public static Command Create(string name,
                                 string description,
                                 CommandType type,
                                 Action<Shell>? action) => new(name, description, type, action);
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