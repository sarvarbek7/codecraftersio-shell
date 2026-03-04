class Program
{
    static void Main()
    {
        IReadOnlyList<string> validCommands = ["clear", "exit", "echo"];

        while (true)
        {
            Console.Write("$ ");

            var input = Console.ReadLine() ?? string.Empty;

            var commandParts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var command = commandParts[0];

            if (!validCommands.Contains(command))
            {
                Console.WriteLine($"{input}: command not found");
            }


            switch (commandParts[0])
            {
                case "clear":
                    Console.Clear();
                    break;
                case "exit":
                    return;
                case "echo":
                    Console.WriteLine(string.Join(' ', commandParts.Skip(1)));
                    break;
            }
        }
    }
}
