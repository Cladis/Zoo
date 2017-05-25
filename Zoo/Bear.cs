using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    class Bear : Animal
    {
        static Bear()
        {
            MaxHealth = 6;
        }

        private Bear() : base()
        {

        }

        public Bear(string name) : base(name)
        {

        }
    }
}
