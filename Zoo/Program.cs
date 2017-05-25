using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Input;

namespace Zoo
{
    class Program
    {
        static List<Animal> Zoo { get; set; } = new List<Animal>();

        static void Main(string[] args)
        {
            ShowWelcomeMessage();
            ShowCommands();

            //  DispatcherTimer setup
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler((sender, e) =>
            {
                Console.WriteLine("Tick!");
                Random rand = new Random();
                int heads = Zoo.Count;
                if (heads > 0)
                {
                    Animal animal = Zoo[rand.Next(Zoo.Count)];
                    animal.WorsenState();
                }
                if (heads == 0 || (from beast in Zoo where beast.State == AnimalState.Dead select beast).Count() == heads)
                {
                    dispatcherTimer.Stop();
                    Console.WriteLine("You've starved all the animals to death!");
                    Console.WriteLine("*** GAME OVER***");
                }
                CommandManager.InvalidateRequerySuggested();
            });
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();
            Console.WriteLine("Gotten so far");

            Console.ReadLine();

        }

        static void ShowWelcomeMessage()
        {

            Console.WriteLine("**** Welcome to the Zoo ****");
        }


        static void ShowCommands()
        {
            //TODO move into a separate class and split into some per key methods
            Console.WriteLine("Commands and input format:");
            Console.WriteLine("add\t\tMakes zoo buy a healthy well fed animal");
            Console.WriteLine("\tadd -name Winnie the Pooh -species Bear");
            Console.WriteLine("feed\t\tFeeds an animal if it is hungry");
            Console.WriteLine("\tfeed -name Tigger ");
            Console.WriteLine("\tfeed Tigger");
            Console.WriteLine("cure\t\tCures an ill animal");
            Console.WriteLine("\tcure -name Tigger ");
            Console.WriteLine("\tcure Tigger ");
            Console.WriteLine("bury\t\tRemoves the animal from the zoo");
            Console.WriteLine("bury -name Cecil");
            Console.WriteLine("bury Cecil");

        }
    }
}
