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
        public byte Health
        {
            get
            {
                return Health;
            }
            protected set
            {
                if (value > MaxHealth)
                {
                    Health = MaxHealth;
                }
                else if (value <= 0)
                {
                    Health = 0;
                    State = AnimalState.Dead;
                }
                else
                {
                    Health = value;
                }
            }
        }
        public AnimalState State { get; protected set; } = AnimalState.Full;
        public String Name { get; set; }

        

        public void Feed()
        {
            if (State == AnimalState.Hungry)
            {
                State = AnimalState.Full;
            }
        }

        public void Cure()
        {
            if (State == AnimalState.Ill)
            {
                // QUESTION: Is it supposed to become Hungry or Full?
                Health++;
                State = AnimalState.Hungry;
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
    }



}
