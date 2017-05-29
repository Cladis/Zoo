using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Zoo
{
    public class Zoo
    {
        Dictionary<string, Animal> Animals { get; } = new Dictionary<string, Animal>();
        bool gameOn = true;
        private readonly Timer _timer = new Timer(5000);
        private readonly object gameLock = new object();
        private readonly object timerLock = new object();


        public void Start()
        {
            _timer.Elapsed += async (sender, e) => await HandleTimer(sender, e);
            lock (timerLock)
            {
                _timer.Start();
            }
            ShowWelcomeMessage();
            var commands = InitialiseCommands();
            commands.PrintCommands();
            var _gameOn = gameOn;
            while (_gameOn)
            {
                commands.ParseCommand(Console.ReadLine());
                lock (gameLock)
                {
                    _gameOn = gameOn;
                }
            }
        }





        void ShowWelcomeMessage()
        {
            Console.WriteLine("**** Welcome to the Zoo ****");
        }



        ConsoleCommands InitialiseCommands()
        {
            var commands = ConsoleCommands.Instance;
            commands.Add(
                new ConsoleCommand(
                    "add",
                    new[] { "name", "species" },
                    "Makes zoo buy a healthy well fed animal",
                    (dict =>
                    {
                        var beast = AnimalFactory.BuyAnimal(dict["name"], dict["species"]);
                        if (beast != null)
                        {
                            Animals.Add(dict["name"], beast);
                            Console.WriteLine($"With the lates acquisition there are {Animals.Count} animals in our Zoo");
                        }
                    }),
                    new[] { "add --name Winnie the Pooh --species Bear" }
                )
            );
            commands.Add(
                new ConsoleCommand(
                    "feed",
                    new[] { "name" },
                    "Feeds an animal if it is hungry",
                    (dict =>
                    {
                        if (Animals.ContainsKey(dict["name"])) { Animals[dict["name"]].Feed(); }
                        else
                        {
                            Console.WriteLine("There's no animal called {0} in the Zoo now", dict["name"]);
                        }
                    }),
                    new[] { "feed --name Tigger", "feed Tigger" }
                )
            );
            commands.Add(
                new ConsoleCommand(
                    "cure",
                    new[] { "name" },
                    "Cures an ill animal",
                    (dict =>
                    {
                        if (Animals.ContainsKey(dict["name"])) { Animals[dict["name"]].Cure(); }
                        else
                        {
                            Console.WriteLine("There's no animal called {0} in the zoo now", dict["name"]);
                        }
                    }),
                    new[] { "cure --name Tigger", "cure Tigger" }
                )
            );
            commands.Add(
                new ConsoleCommand(
                    "bury",
                    new[] { "name" },
                    "Removes the animal from the zoo",
                    (dict =>
                    {
                        if (Animals.ContainsKey(dict["name"]))
                        {
                            var beast = Animals[dict["name"]];
                            if (beast.State == AnimalState.Dead)
                            {
                                Animals.Remove(dict["name"]);
                                Console.WriteLine($"{beast} was buried. There are still {Animals.Count} in the Zoo");
                            }
                            else
                            {
                                Console.WriteLine($"{beast} is not dead, you monster!");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"There's no animal called {dict["name"]} in the zoo now");
                        }
                    }),
                    new[] { "bury --name Cecil", "bury Cecil" }
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
                    new[] { "help" }
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
                    new[] { "quit" }
                )
            );

            return commands;
        }



        private void GameOver(string reason = "You lost")
        {
            lock (timerLock)
            {
                _timer.Stop();
            }
            Console.WriteLine(reason);
            Console.WriteLine("*** GAME OVER ***");
            Animals.Clear();
            ConsoleCommands.Commands.Clear();

        }


        private Task HandleTimer(object sender, ElapsedEventArgs e)
        {
            return Task.Run(() =>
            {
                var heads = Animals.Count;
                if (heads > 0)
                {
                    var rand = new Random();
                    var animal = Animals.Values.ToList()[rand.Next((Animals.Count))];
                    animal.WorsenState();
                    if (heads == 0 || (from beast in Animals.Values where beast.State == AnimalState.Dead select beast).Count() == heads)
                    {
                        GameOver("You've starved all the animals to death!");
                    }


                }
            });

        }
    } // end of class

}