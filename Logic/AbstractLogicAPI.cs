using Data;

namespace Logic
{
    public abstract class AbstractLogicAPI
    {
        public static AbstractLogicAPI createInstance(AbstractDataAPI? dataAPI = null)
        {
            return new LogicAPI(dataAPI ?? AbstractDataAPI.createInstance());
        }

        public abstract List<IBoat> GetAllBoats();
        public abstract void addBoat(string name, string description, float price);
        public abstract bool buyBoat(int id);
        public abstract event Action OnPriceUpdated;

        private class LogicAPI : AbstractLogicAPI
        {
            private readonly AbstractDataAPI myDataAPI;
            private int IdCounter = 1;
            public override event Action OnPriceUpdated;

            public LogicAPI(AbstractDataAPI? dataAPI)
            {
                myDataAPI = dataAPI;
                IdCounter = dataAPI.GetAllBoats().Count + 1;
            }

            public override List<IBoat> GetAllBoats()
            {
                return myDataAPI.GetAllBoats();
            }

            public override void addBoat(string name, string description, float price)
            {
                myDataAPI.AddBoat(IdCounter++, name, description, price);
            }

            public override bool buyBoat(int id)
            {
                var boat = myDataAPI.GetBoatById(id);
                if (boat == null)
                {
                    return false;
                }

                Console.WriteLine($"Boat {boat.Name} has been bought");
                
                myDataAPI.RemoveBoat(id);
                return true;
            }

            public void ApplyDiscount()
            {
                var currentTime = DateTime.Now.Hour;

                foreach (var boat in myDataAPI.GetAllBoats())
                {
                    // Apply 50% discount between 14:00 and 18:00
                    if (currentTime >= 14 && currentTime < 18)
                    {
                        boat.Price *= 0.5f;  // Apply 50% discount
                    }
                    else
                    {
                        boat.Price *= 2.0f;  // Restore the original price
                    }
                }

                OnPriceUpdated?.Invoke();  // Trigger the event after price update
            }
        }

    }
}
