namespace Zoo
{
    class Elephant : Animal
    {
        static Elephant()
        {
            MaxHealth = 7;
        }

        private Elephant()
        {

        }

        public Elephant(string name) : base(name)
        {

        }
    }
}
