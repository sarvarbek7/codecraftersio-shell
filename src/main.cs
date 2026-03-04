class Program
{
    static void Main()
    {
        IReadOnlyList<string> validCommands = ["clear", "exit"];

        while (true)
        {
            Console.Write("$ ");

            var input = Console.ReadLine();

            if (!validCommands.Contains(input))
            {
                Console.WriteLine($"{input}: command not found");
            }

            switch (input)
            {
                case "clear":
                    Console.Clear();
                    break;
                case "exit":
                    return;
            }
        }
    }
}
