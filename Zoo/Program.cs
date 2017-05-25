using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Zoo
{
    class Program
    {
        /// <summary>
        /// TODO encapsulate into a separate Zoo class
        /// </summary>
        static Dictionary<string, Animal> Zoo { get; set; } = new Dictionary<string, Animal>();
        static bool gameOn = true;
        static Timer timer = new Timer(5000);
        private static readonly object gameLock = new object();
        private static readonly object timerLock = new object();

        static void Main(string[] args)
        {
            timer.Elapsed += async (sender, e) => await HandleTimer(sender, e);
            timer.Start();
            ShowWelcomeMessage();
            ConsoleCommands commands = InitialiseCommands();
            commands.PrintCommands();
            bool _gameOn = gameOn;
            while (_gameOn)
            {
                    commands.ParseCommand(Console.ReadLine());
                lock (gameLock) {
                    _gameOn = gameOn;
                }
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
                        Animal beast = Zoo[dict["name"]];
                        if (beast.State == AnimalState.Dead)
                        {
                            Zoo.Remove(dict["name"]);
                            Console.WriteLine("{0} was buried. There are still {1} in the Zoo", beast, Zoo.Count());
                        }
                        else
                        {
                            Console.WriteLine("{0} is not dead, you monster!", beast);
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
                }),
                    new string[] { "quit" }
                )
            );

            return commands;
        }

        private static void GameOver(string reason = "You lost")
        {
            lock (timerLock)
            {
            timer.Stop();
            }
            lock (gameLock)
            {
                gameOn = false;
            }
                Console.WriteLine(reason);
                Console.WriteLine("*** GAME OVER ***");
        }


        private static Task HandleTimer(object sender, ElapsedEventArgs e)
        {
            return Task.Run(() =>
            {
                int heads = Zoo.Count;
                if (heads > 0)
                {
                    Random rand = new Random();
                    Animal animal = Enumerable.ToList<Animal>(Zoo.Values)[rand.Next((Zoo.Count))];
                    animal.WorsenState();
                    if (heads == 0 || (from beast in Zoo.Values where beast.State == AnimalState.Dead select beast).Count() == heads)
                    {
                        GameOver("You've starved all the animals to death!");
                    }
                    

                }
            });
            
        }
    }
}

