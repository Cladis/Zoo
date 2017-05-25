using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    static class AnimalFactory
    {
        static Animal BuyAnimal(String name, String species)
        {
            switch (species)
            {
                case "bear":
                    return new Bear(name);
            }
            return null;
        }
    }
}
