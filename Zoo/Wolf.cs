using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Zoo
{
    class Wolf : Animal
    {
        static Wolf()
        {
            MaxHealth = 4;
        }

        private Wolf() : base()
        {

        }

        public Wolf(string name) : base(name)
        {

        }
    }
}
