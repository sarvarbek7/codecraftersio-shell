class Program
{
    static void Main()
    {

        while (true)
        {
            Console.Write("$ ");

            var input = Console.ReadLine() ?? string.Empty;

            var commandParts = CommandExtractor.ExtractCommand(input);

            if (!Commands.IsValidCommand(commandParts.Command))
            {
                Console.WriteLine($"{commandParts.Command}: command not found");
                continue;
            }

            if (commandParts.Command == Commands.Exit.Name)
            {
                break;
            }
            else if (commandParts.Command == Commands.Clear.Name)
            {
                Console.Clear();
            }
            else if (commandParts.Command == Commands.Echo.Name)
            {
                Console.WriteLine(string.Join(' ', commandParts.Arguments));
            }
            else if (commandParts.Command == Commands.Type.Name)
            {
                var cmdName = commandParts.Arguments.FirstOrDefault();
                if (cmdName == null)
                {
                    Console.WriteLine("type: missing argument");
                    continue;
                }

                var cmd = Commands.GetCommand(cmdName);
                if (cmd == null)
                {
                    Console.WriteLine($"{cmdName}: not found");
                    continue;
                }
                if (cmd.Type == CommandType.BuiltIn)
                {
                    Console.WriteLine($"{cmdName} is a shell builtin");
                }
                else
                {
                    Console.WriteLine($"{cmdName} is an external command");
                }
            }
        }
    }
}
