namespace Zoo
{
    class Bear : Animal
    {
        static Bear()
        {
            MaxHealth = 6;
        }

        private Bear()
        {

        }

        public Bear(string name) : base(name)
        {

        }
    }
}
