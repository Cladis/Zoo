using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    class Program
    {
        static List<Animal> Zoo { get; set; } = new List<Animal>();

        static void Main(string[] args)
        {
            ShowWelcomeMessage();
            ShowCommands();
            String lol = Console.ReadLine();
            Console.WriteLine(lol);
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
