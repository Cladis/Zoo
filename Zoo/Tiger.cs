namespace Zoo
{
    class Tiger : Animal
    {
        static Tiger()
        {
            MaxHealth = 4;
        }

        private Tiger()
        {

        }

        public Tiger(string name) : base(name)
        {

        }
    }
}
