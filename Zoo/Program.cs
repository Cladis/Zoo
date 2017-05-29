using System;

namespace Zoo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var game = new Zoo();
            game.Start();
            game.Outro();
            Console.ReadLine();
        }
    }
}

