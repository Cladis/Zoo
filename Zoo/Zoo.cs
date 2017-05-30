using System;
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
                        if (beast == null) return;
                        if (!_animals.Contains(dict["name"]))
                        {
                            _animals.Add(dict["name"], beast);
                            Console.WriteLine(
                                $"With the lates acquisition there are {_animals.Count} animals in our Zoo");
                        }
                        else
                        {
                            Console.WriteLine(
                                $"Your fantasy sucks. There's already an animal called {dict["name"]} in the Zoo!");
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
                    new[] { "restart" }
                )
            );
            commands.Add(
                new ConsoleCommand(
                    "populate",
                    new string[] { },
                    "A test-only command populating the Zoo with 5 pre-set animals",
                    (dict =>
                    {
                        commands.ParseCommand("add --name Baloo --species Bear");
                        commands.ParseCommand("add --name Hathi --species Elephant");
                        commands.ParseCommand("add --name Shere Khan --species Tiger");
                        commands.ParseCommand("add --name Cecil --species Lion");
                        commands.ParseCommand("add --name Akela --species Wolf");
                    }),
                    new[] { "populate" }
                )
            );
            commands.Add(
                new ConsoleCommand(
                    "select-all",
                    new string[] { },
                    "Selects all the animals in the Zoo now",
                    (dict =>
                    {
                        _animals.PrintListOfAnimals(_animals.GetAllAnimals());
                    }),
                    new[] { "selectall" }
                )
            );
            commands.Add(
                new ConsoleCommand(
                    "select-state",
                    new[] { "state"},
                    "Selects all the animals in the Zoo with the state given",
                    (dict =>
                    {
                        _animals.PrintListOfAnimals(_animals.GetAnimalsByState(dict["state"]));
                    }),
                    new[] { "select-state --state Hungry", "select-state Hungry" }
                    )
            );
            commands.Add(
                new ConsoleCommand(
                    "select-sc",
                    new[] { "state" },
                    "Gives the number of animals in the Zoo with the state given",
                    (dict =>
                    {
                        Console.WriteLine($"There are currently {_animals.CountAnimalsByState(dict["state"])} {dict["state"]} animals");
                    }),
                    new[] { "select-sc --state Hungry", "select-state Hungry" }
                )
            );
            commands.Add(
                new ConsoleCommand(
                    "select-ss",
                    new[] { "state", "species"},
                    "Selects all the animals in the Zoo with the state given of the species given",
                    (dict =>
                    {
                        _animals.PrintListOfAnimals(_animals.GetAnimalsByStateBySpecies(dict["state"], dict["species"]));
                    }),
                    new[] { "select-ss --state Hungry --species Lion" }
                )
            );
            commands.Add(
                new ConsoleCommand(
                    "select-name",
                    new[] { "name" },
                    "Selects the animal specified",
                    (dict =>
                    {
                        if (!_animals.Contains(dict["name"]))
                        {
                            Console.WriteLine($"There's no animal called {dict["name"]} in the Zoo");
                            return;
                        }
                        Console.WriteLine(_animals[dict["name"]]);
                    }),
                    new[] { "select-name --name Cecil", "select-name Cecil" }
                )
            );
            commands.Add(
                new ConsoleCommand(
                    "select-sh",
                    new[] { "species", "threshold" },
                    "Selects the animals of the species specified with health > than threshold specified",
                    (dict =>
                    {
                        byte threshold;
                        try
                        {
                            threshold = byte.Parse(dict["threshold"]);
                        }
                        catch
                        {
                            Console.WriteLine("Could not parse your threshold. A digit. You know what it is, right?");
                            return;
                        }
                        _animals.PrintListOfAnimals(
                            _animals.GetHealthierThan(dict["species"].ToLower().Split('|'),
                            threshold)
                            );
                    }),
                    new[] { "select-sh --species Wolf|Bear --threshold 3" }
                )
            );
            commands.Add(
                new ConsoleCommand(
                    "average-health",
                    new string[] { },
                    "Prints average health of all the animals",
                    (dict =>
                    {
                        Console.WriteLine($"The average health of all the animals is {_animals.GetAverageHealth():0.####}");
                    }),
                    new[] { "average-health" }
                )
            );
            commands.Add(
                new ConsoleCommand(
                    "select-minmax",
                    new string[] { },
                    "Prints the animal with the minimum health followed by the animal with the maximum health",
                    (dict =>
                    {
                        _animals.PrintListOfAnimals(_animals.MinMaxHealth());
                    }),
                    new[] { "select-minmax" }
                )
            );
            commands.Add(
                new ConsoleCommand(
                    "select-healthiest",
                    new string[] { },
                    "Prints the healthiest animals of each species",
                    (dict =>
                    {
                        _animals.PrintListOfAnimals(_animals.GetMostHealthy());
                    }),
                    new[] { "select-healthiest" }
                )
            );
            commands.Add(
                new ConsoleCommand(
                    "run-examples",
                    new string[] { },
                    "Shows and runs example queries",
                    (dict =>
                    {
                        commands.DemonstrateCommands("select-all");
                        commands.DemonstrateCommands("select-ss --species Tiger --state Ill");
                        commands.DemonstrateCommands("select-name Hathi");
                        commands.DemonstrateCommands("select-state Hungry");
                        commands.DemonstrateCommands("select-healthiest");
                        commands.DemonstrateCommands("select-sc Dead");
                        commands.DemonstrateCommands("select-sh --species Wolf|Bear --threshold 3");
                        commands.DemonstrateCommands("select-minmax");
                        commands.DemonstrateCommands("average-health");
                    }),
                    new[] { "run-examples" }
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