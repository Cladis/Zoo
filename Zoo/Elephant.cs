using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    class Elephant : Animal
    {
        static Elephant()
        {
            MaxHealth = 7;
        }

        private Elephant() : base()
        {

        }

        public Elephant(string name) : base(name)
        {

        }
    }
}
