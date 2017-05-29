namespace Zoo
{
    class Lion : Animal
    {
        static Lion()
        {
            MaxHealth = 5;
        }

        private Lion()
        {

        }

        public Lion(string name) : base(name)
        {

        }
    }
}
