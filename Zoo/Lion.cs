using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    class Lion : Animal
    {
        static Lion()
        {
            MaxHealth = 5;
        }

        private Lion() : base()
        {

        }

        public Lion(string name) : base(name)
        {

        }
    }
}
