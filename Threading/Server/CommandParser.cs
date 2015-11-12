using System;
using System.Linq;

namespace Server
{
    public static class CommandParser
    {
        public static ICommand Parse(string text)
        {
            var args = text.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
            if (!args.Any())
            {
                throw new CommandParseException("Cannot process empty command");
            }
            switch (args[0])
            {
            case "create":
                return ParseCreateCommand(args);
            case "start":
                return ParseStartCommandOptions(args);
            case "stop":
                return ParseStopCommandOptions(args);
            default:
                throw new CommandParseException($"Unknown command: {args[0]}");
            }
        }

        private static CreateCommand ParseCreateCommand(string[] args)
        {
            var command = new CreateCommand();
            for (var i = 1; i < args.Length; i++)
            {
                switch (args[i])
                {
                case "--start":
                case "-s":
                    command.StartUponCreation = true;
                    break;
                case "--in":
                case "-i":
                    if (i + 1 == args.Length)
                    {
                        throw new CommandParseException("Unexpected command end");
                    }
                    int delay;
                    if (int.TryParse(args[i + 1], out delay))
                    {
                        command.Delay = TimeSpan.FromSeconds(delay);
                        i++;
                        continue;
                    }
                    throw new CommandParseException($"Cannot parse start delay: {args[i + 1]}");
                case "--after":
                case "-a":
                    if (i + 1 == args.Length)
                    {
                        throw new CommandParseException("Unexpected command end");
                    }
                    int taskId;
                    if (int.TryParse(args[i + 1], out taskId))
                    {
                        command.DependentTaskIds.Add(taskId);
                        i++;
                        continue;
                    }
                    throw new CommandParseException($"Cannot parse dependent task id: {args[i]}");
                default:
                    int steps;
                    if (int.TryParse(args[i], out steps))
                    {
                        command.Steps = steps;
                        continue;
                    }
                    throw new CommandParseException($"Cannot parse parameter: {args[i]}");
                }
            }

            return command;
        }

        private static StartCommand ParseStartCommandOptions(string[] args)
        {
            int id;
            if (args.Length == 2 && int.TryParse(args[1], out id))
            {
                return new StartCommand {TaskId = id};
            }
            throw new CommandParseException("Cannot parse start command");
        }

        private static StopCommand ParseStopCommandOptions(string[] args)
        {
            int id;
            if (args.Length == 2 && int.TryParse(args[1], out id))
            {
                return new StopCommand {TaskId = id};
            }
            throw new CommandParseException("Cannot parse stop command");
        }
    }
}