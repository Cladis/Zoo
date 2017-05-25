using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    class Fox : Animal
    {
        static Fox()
        {
            MaxHealth = 3;
        }

        private Fox() : base()
        {

        }

        public Fox(string name) : base(name)
        {

        }
    }
}
