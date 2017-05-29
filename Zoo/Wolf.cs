namespace Zoo
{
    class Wolf : Animal
    {
        static Wolf()
        {
            MaxHealth = 4;
        }

        private Wolf()
        {

        }

        public Wolf(string name) : base(name)
        {

        }
    }
}
