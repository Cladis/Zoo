using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zoo
{
    /// <summary>
    /// Encapsulates list of console commands supported by the application
    /// </summary>
    class ConsoleCommands
    {
        static public ConsoleCommands Instance { get; }

        static public List<ConsoleCommand> Commands { get; set; }

        private ConsoleCommands()
        {

        }

        static ConsoleCommands()
        {
            Instance = new ConsoleCommands();
            Commands = new List<ConsoleCommand>();
        }

        public void Add(ConsoleCommand command)
        {
            Commands.Add(command);
        }

        public void Add(ConsoleCommand[] commands)
        {
            Commands.AddRange(commands);
        }

        public void PrintCommands()
        {
            Commands.ForEach(command => { command.Print(); });
        }

        public void DescribeCommands()
        {
            Commands.ForEach(command => { command.Describe(); });
        }

        public void ParseCommand(string input)
        {
            input = input.Trim();
            var key = Regex.Split(input, @"\s")[0].ToLower();
            var command = (from comm in Commands where comm.Key == key select comm).FirstOrDefault();
            if (command != null)
            {
                var keyPairs = Regex.Split(input.Substring(key.Length).Trim(), @"(\-{2}[a-z]+)");
                var pairsCount = (from pair in keyPairs where !pair.Equals(String.Empty) select pair).Count();
                if (pairsCount == 1)
                {
                    if (command.Flags.Count == 1)
                    {
                        command.FlagValues.Add(command.Flags[0], keyPairs[0]);
                    }
                    else
                    {
                        Console.WriteLine("Wrong input format for '{0}': it has more than one key, could not disambiguate", command.Key);
                        command.Describe();
                        command = null;
                    }
                }
                else if (pairsCount == ( command.Flags.Count() * 2))
                {
                    for (var i = 1; i<= pairsCount; i+=2)
                    {
                        var flag = keyPairs[i].ToLower().Substring("--".Length);
                        var value = keyPairs[i + 1];
                        if ( !command.Flags.Contains(flag))
                        {
                            Console.WriteLine("Wrong input format for '{0}': check the keys format", command.Key);
                            command.Describe();
                            command = null;
                            break;
                        }
                        command.FlagValues.Add(flag.Trim().ToLower(), value.Trim().ToLower());
                    }
                }
                else
                {
                    Console.WriteLine("Wrong input format for '{0}': all the flags are compulsory, extra keys are not allowed", command.Key);
                    command.Describe();
                    command = null;
                }
            }
            else
            {
                Console.WriteLine("Command not recognised");
                PrintCommands();
                command = null;
            }
            if (command != null)
            {
                command.Action(command.FlagValues);
                command.FlagValues.Clear();
            }
        }
    }
}
