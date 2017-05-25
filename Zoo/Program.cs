using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    class Program
    {
        /// <summary>
        /// TODO encapsulate into a separate Zoo class
        /// </summary>
        static Dictionary<string, Animal> Zoo { get; set; } = new Dictionary<string, Animal>();
        static bool gameOn = true;

        static void Main(string[] args)
        {
            ShowWelcomeMessage();
            ConsoleCommands commands = InitialiseCommands();
            commands.PrintCommands();
            while (gameOn)
            {
                commands.ParseCommand(Console.ReadLine());
            }
            Console.ReadLine();

        }

        static void ShowWelcomeMessage()
        {
            Console.WriteLine("**** Welcome to the Zoo ****");
        }


        static ConsoleCommands InitialiseCommands()
        {
            ConsoleCommands commands = ConsoleCommands.Instance;
            commands.Add(
                new ConsoleCommand(
                    "add",
                    new string[] { "name", "species" },
                    "Makes zoo buy a healthy well fed animal",
                    (dict =>
                    {
                        Animal beast = AnimalFactory.BuyAnimal(dict["name"], dict["species"]);
                        if (beast != null)
                        {
                            Zoo.Add(dict["name"], beast);
                            Console.WriteLine("With the lates acquisition there are {0} animals in our Zoo", Zoo.Count());
                        }
                    }),
                    new string[] { "add --name Winnie the Pooh --species Bear" }
                )
            );
            commands.Add(
                new ConsoleCommand(
                    "feed",
                    new string[] { "name" },
                    "Feeds an animal if it is hungry",
                    (dict =>
                    {
                        if (Zoo.ContainsKey(dict["name"])) { Zoo[dict["name"]].Feed(); }
                        else
                        {
                            Console.WriteLine("There's no animal called {0} in the zoo now", dict["name"]);
                        }
                    }),
                    new string[] { "feed --name Tigger", "feed Tigger" }
                )
            );
            commands.Add(
            new ConsoleCommand(
                "cure",
                new string[] { "name" },
                "Cures an ill animal",
                (dict =>
                {
                    if (Zoo.ContainsKey(dict["name"])) { Zoo[dict["name"]].Cure(); }
                    else
                    {
                        Console.WriteLine("There's no animal called {0} in the zoo now", dict["name"]);
                    }
                    }),
                    new string[] { "cure --name Tigger", "cure Tigger" }
                )
            );
            commands.Add(
            new ConsoleCommand(
                "bury",
                new string[] { "name" },
                "Removes the animal from the zoo",
                (dict =>
                {
                    if (Zoo.ContainsKey(dict["name"]))
                    {
                        if (Zoo[dict["name"]].State == AnimalState.Dead)
                        {
                            Zoo.Remove(dict["name"]);
                            Console.WriteLine("{0} was buried. There are still {1} in the Zoo", Zoo[dict["name"]], Zoo.Count());
                        }
                        else
                        {
                            Console.WriteLine("{0} is not dead, you monster!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("There's no animal called {0} in the zoo now", dict["name"]);
                    }
                    }),
                    new string[] { "bury --name Cecil", "bury Cecil" }
                )
            );
            commands.Add(
            new ConsoleCommand(
                "help",
                new string[] { },
                "Displays this help",
                (dict =>
                {
                    commands.PrintCommands();
                }),
                    new string[] { "help" }
                )
            );
            commands.Add(
            new ConsoleCommand(
                "quit",
                new string[] { },
                "Quit the game. Means you are giving up",
                (dict =>
                {
                    GameOver("You gave up");
                    gameOn = false;
                }),
                    new string[] { "quit" }
                )
            );

            return commands;
        }

        private static void GameOver(string reason = "You lost")
        {
            Console.WriteLine(reason);
            Console.WriteLine("*** GAME OVER ***");
        }
    }
}
