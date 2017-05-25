using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    static class AnimalFactory
    {
        public static Animal BuyAnimal(String name, String species)
        {
            Animal animal = null;
            switch (species)
            {
                case "bear":
                    animal = new Bear(name);
                    break;
                case "elephant":
                    animal = new Elephant(name);
                    break;
                case "fox":
                    animal = new Fox(name);
                    break;
                case "lion":
                    animal = new Lion(name);
                    break;
                case "tiger":
                    animal = new Tiger(name);
                    break;
                case "wolf":
                    animal = new Wolf(name);
                    break;
                default:
                    Console.WriteLine("The species of {0} is unknown to our biologists", species);
                    break;
            }
            if (animal != null) {
                Console.WriteLine("You've just bought {0}", animal);
            }
            return animal;
        }
    }
}
