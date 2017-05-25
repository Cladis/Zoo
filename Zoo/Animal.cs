using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    abstract class Animal
    {
        public static byte MaxHealth { get; protected set; }
        private byte health;
        public byte Health
        {
            get
            {
                return health;
            }
            protected set
            {
                if (value > MaxHealth)
                {
                    health = MaxHealth;
                }
                else if (value <= 0)
                {
                    health = 0;
                    State = AnimalState.Dead;
                }
                else
                {
                    health = value;
                }
            }
        }
        public AnimalState State { get; protected set; } = AnimalState.Full;
        public string Name { get; set; }

        public Animal()
        {

        }

        public Animal(string name)
        {
            Name = name;
            Health = MaxHealth;
        }

        public void Feed()
        {
            if (State == AnimalState.Hungry)
            {
                State = AnimalState.Full;
                Console.WriteLine("{0} was fed", this);
            }
        }

        public void Cure()
        {
            if (State == AnimalState.Ill)
            {
                // QUESTION: Is it supposed to become Hungry or Full?
                Health++;
                State = AnimalState.Hungry;
                Console.WriteLine("{0} was cured", this);
            }
        }

        public void WorsenState()
        {
            switch (State)
            {
                case AnimalState.Full:
                    State = AnimalState.Hungry;
                    break;
                case AnimalState.Hungry:
                    State = AnimalState.Ill;
                    break;
                case AnimalState.Ill:
                    Health--;
                    break;
            }
        }

        public override string ToString()
        {
            return String.Format("{0} the {1}, state: {2}, health {3}", Name, GetType().Name, State, Health);
        }
    }



}
