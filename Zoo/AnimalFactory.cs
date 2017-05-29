using System;

namespace Zoo
{
    internal static class AnimalFactory
    {
        public static Animal BuyAnimal(string name, string species)
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
                    Console.WriteLine($"The species of {species} is unknown to our biologists");
                    break;
            }
            if (animal != null) {
                Console.WriteLine($"You've just bought {animal}");
            }
            return animal;
        }
    }
}
