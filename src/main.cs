class Program
{
    static void Main()
    {
        IReadOnlyList<string> validCommands = ["clear"];

        while (true)
        {
            Console.Write("$ ");

            var input = Console.ReadLine();

            if (!validCommands.Contains(input))
            {
                Console.WriteLine($"{input}: command not found");
            }
        }
    }
}
