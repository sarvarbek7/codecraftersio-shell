public static class CommandExtractor
{
    public static CommandParts ExtractCommand(string input)
    {
        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return new CommandParts(parts[0], parts.Skip(1));
    }
}

public record CommandParts(string Command, IEnumerable<string> Arguments);