namespace Zoo
{
    class Fox : Animal
    {
        static Fox()
        {
            MaxHealth = 3;
        }

        private Fox()
        {

        }

        public Fox(string name) : base(name)
        {

        }
    }
}
