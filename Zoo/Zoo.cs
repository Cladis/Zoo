using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Zoo
{
    public class Zoo
    {
        private readonly ZooRepository _animals = new ZooRepository();
        private bool _gameOn = true;
        private readonly Timer _timer = new Timer(5000);
        private readonly object _gameLock = new object();
        private readonly object _timerLock = new object();

        public void Start()
        {
            _timer.Elapsed += async (sender, e) => await HandleTimer(sender, e);
            lock (_timerLock)
            {
                _timer.Start();
            }
            PrintWelcomeMessage();
            var commands = InitialiseCommands();
            commands.PrintCommands();
            var gameOn = _gameOn;
            while (gameOn)
            {
                commands.ParseCommand(Console.ReadLine());
                lock (_gameLock)
                {
                    gameOn = _gameOn;
                }
            }
        }

        private static void PrintWelcomeMessage()
        {
            Console.WriteLine("**** Welcome to the Zoo ****");
        }

        private ConsoleCommands InitialiseCommands()
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
                            _animals.Add(dict["name"], beast);
                            Console.WriteLine($"With the lates acquisition there are {_animals.Count} animals in our Zoo");
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
                        if (_animals.Contains(dict["name"])) { _animals[dict["name"]].Feed(); }
                        else
                        {
                            Console.WriteLine($"There's no animal called {dict["name"]} in the Zoo now");
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
                        if (_animals.Contains(dict["name"])) { _animals[dict["name"]].Cure(); }
                        else
                        {
                            Console.WriteLine($"There's no animal called {dict["name"]} in the zoo now");
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
                        if (_animals.Contains(dict["name"]))
                        {
                            var beast = _animals[dict["name"]];
                            if (beast.State == AnimalState.Dead)
                            {
                                _animals.Delete(dict["name"]);
                                Console.WriteLine($"{beast} was buried. There are still {_animals.Count} in the Zoo");
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
            commands.Add(
                new ConsoleCommand(
                    "restart",
                    new string[] { },
                    "Restart the game. Means you are giving up",
                    (dict =>
                    {
                        GameOver("You gave up", true);
                    }),
                    new[] { "quit" }
                )
            );

            return commands;
        }

        private void GameOver(string reason = "You lost", bool restart = false)
        {
            _animals.Clear();

            Console.WriteLine(reason);
            Console.WriteLine("*** GAME OVER ***");
            if (restart) return;
            lock (_timerLock)
            {
                _timer.Stop();
                _timer.Dispose();
            }
            lock (_gameLock)
            {
                _gameOn = false;
            }
        }

        private Task HandleTimer(object sender, ElapsedEventArgs e)
        {
            return Task.Run(() =>
            {
                var heads = _animals.Count;
                if (heads == 0) return;

                var rand = new Random();
                var aliveAnimals = _animals.GetAliveAnimals();
                if (aliveAnimals.Count > 0)
                {
                    var animal = aliveAnimals[rand.Next(aliveAnimals.Count)];
                    animal.WorsenState();
                }
                if (_animals.GetDeadAnimals().Count == heads)
                {
                    GameOver("You've starved all the animals to death!");
                }
            });
        }


        public void Outro()
        {
            Console.WriteLine("*** Thank you for playing the Zoo ***");
        }
    } // end of class
}// end of namespace