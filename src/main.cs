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
                Console.WriteLine($"{commandParts.Command}: command not found");
                continue;
            }

            command.Action(commandParts.Arguments);
        }
    }
}
