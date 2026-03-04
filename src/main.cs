using System.Diagnostics;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Write("$ ");

            var input = Console.ReadLine() ?? string.Empty;

            var commandParts = CommandExtractor.ExtractCommand(input);

            var command = Commands.GetCommand(commandParts.Command);

            if (command == null)
            {
                if (Helpers.GetExecutableCommandPath(commandParts.Command) != null)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = commandParts.Command,
                        Arguments = string.Join(' ', commandParts.Arguments),
                        UseShellExecute = true
                    });
                    
                    continue;
                }

                Console.WriteLine($"{commandParts.Command}: command not found");
                continue;
            }

            command.Action(commandParts.Arguments);
        }
    }
}
