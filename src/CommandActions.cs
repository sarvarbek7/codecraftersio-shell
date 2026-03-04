using System.Security.AccessControl;
using Microsoft.VisualBasic;

public static class CommandActions
{
    public static void ExecuteClear() => Console.Clear();

    public static void ExecuteExit() => Environment.Exit(0);

    public static void ExecuteEcho(IEnumerable<string> args) => Console.WriteLine(string.Join(' ', args));

    public static void ExecuteType(IEnumerable<string> args)
    {
        var cmdName = args.FirstOrDefault();

        if (cmdName == null)
        {
            Console.WriteLine("type: missing argument");
            return;
        }

        var cmd = Commands.GetCommand(cmdName);

        if (cmd == null)
        {
            var executablePath = Helpers.GetExecutableCommandPath(cmdName);
            
            if (executablePath != null)
            {
                Console.WriteLine($"{cmdName} is {executablePath}");
                return;
            }

            Console.WriteLine($"{cmdName}: not found");
            return;
        }

        if (cmd.Type == CommandType.BuiltIn)
        {
            Console.WriteLine($"{cmdName} is a shell builtin");
        }
    }

    }