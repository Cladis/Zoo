using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    class Tiger : Animal
    {
        static Tiger()
        {
            MaxHealth = 4;
        }

        private Tiger() : base()
        {

        }

        public Tiger(string name) : base(name)
        {

        }
    }
}
