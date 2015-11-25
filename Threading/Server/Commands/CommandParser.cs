using System;
using System.Linq;

namespace Server.Commands
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
            case "pause":
                return ParsePauseCommandOptions(args);
            case "cancel":
                return ParseCancelCommandOptions(args);
            case "reset":
                return ParseResetCommandOptions(args);
            case "create-parent":
                return ParseCreateParentCommandOptions(args);
            default:
                throw new CommandParseException($"Unknown command: {args[0]}");
            }
        }

        private static CreateParentCommand ParseCreateParentCommandOptions(string[] args)
        {
            var command = new CreateParentCommand();
            for (var i = 1; i < args.Length; i++)
            {
                int id;
                if (int.TryParse(args[i], out id))
                {
                    command.ChildTaskIds.Add(id);
                }
                else
                {
                    throw new CommandParseException("Cannot parse create parent command");
                }
            }
            return command;
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
                        command.DelayInSeconds = delay;
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
                    int iterations;
                    if (int.TryParse(args[i], out iterations))
                    {
                        command.Iterations = iterations;
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

        private static PauseCommand ParsePauseCommandOptions(string[] args)
        {
            int id;
            if (args.Length == 2 && int.TryParse(args[1], out id))
            {
                return new PauseCommand {TaskId = id};
            }
            throw new CommandParseException("Cannot parse pause command");
        }

        private static CancelCommand ParseCancelCommandOptions(string[] args)
        {
            int id;
            if (args.Length == 2 && int.TryParse(args[1], out id))
            {
                return new CancelCommand {TaskId = id};
            }
            throw new CommandParseException("Cannot parse cancel command");
        }

        private static ResetCommand ParseResetCommandOptions(string[] args)
        {
            var command = new ResetCommand();
            for (var i = 1; i < args.Length; i++)
            {
                if (args[i] == "--stop")
                {
                    command.IsStopSet = true;
                    continue;
                }

                int id;
                if (int.TryParse(args[i], out id))
                {
                    command.TaskId = id;
                }
                else
                {
                    throw new CommandParseException("Cannot parse reset command");
                }
            }

            return command;
        }
    }
}