using System;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
    public class Command
    {
        public int? TaskId { get; set; }
        public CommandType Type { get; set; }
        public TimeSpan? Delay { get; set; }
        public bool StartUponCreation { get; set; }
        public List<int> DependentTaskIds { get; set; } = new List<int>();
        public int Duration { get; set; }

        public static Command Parse(string text)
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
            case "pause":
                return ParsePauseCommandOptions(args);
            default:
                throw new CommandParseException($"Unknown command: {args[0]}");
            }
        }

        private static Command ParseCreateCommand(string[] args)
        {
            var command = new Command {Type = CommandType.Create};
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
                    int duration;
                    if (int.TryParse(args[i], out duration))
                    {
                        command.Duration = duration;
                        continue;
                    }
                    throw new CommandParseException($"Cannot parse parameter: {args[i]}");
                }
            }

            return command;
        }

        private static Command ParseStartCommandOptions(string[] args)
        {
            int id;
            if (args.Length == 2 && int.TryParse(args[1], out id))
            {
                return new Command {Type = CommandType.Start, TaskId = id};
            }
            throw new CommandParseException("Cannot parse start command");
        }

        private static Command ParsePauseCommandOptions(string[] args)
        {
            int id;
            if (args.Length == 2 && int.TryParse(args[1], out id))
            {
                return new Command {Type = CommandType.Pause, TaskId = id};
            }
            throw new CommandParseException("Cannot parse stop command");
        }
    }
}