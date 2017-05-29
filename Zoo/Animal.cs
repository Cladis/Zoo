using System;

namespace Zoo
{
    public abstract class Animal
    {
        public byte MaxHealth { get; protected set; }
        private byte _health;
        public byte Health
        {
            get
            {
                return _health;
            }
            protected set
            {
                if (value > MaxHealth)
                {
                    _health = MaxHealth;
                }
                else if (value <= 0)
                {
                    _health = 0;
                    State = AnimalState.Dead;
                }
                else
                {
                    _health = value;
                }
            }
        }
        public AnimalState State { get; protected set; } = AnimalState.Full;
        public string Name { get; set; }


        protected Animal(string name, byte maxHealth)
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = maxHealth;
        }

        public void Feed()
        {
            if (State == AnimalState.Hungry)
            {
                State = AnimalState.Full;
                Console.WriteLine($"{this} was fed");
            }
        }

        public void Cure()
        {
            if (State == AnimalState.Ill)
            {
                // QUESTION: Is it supposed to become Hungry or Full?
                Health++;
                if (Health == MaxHealth)
                {
                    State = AnimalState.Hungry;
                }
                Console.WriteLine($"{this} was cured");
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
            return $"{Name} the {GetType().Name}, state: {State}, health {Health}";
        }
    }



}
