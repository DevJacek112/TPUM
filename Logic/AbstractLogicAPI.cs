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


        private class LogicAPI : AbstractLogicAPI
        {
            private readonly AbstractDataAPI myDataAPI;
            private int IdCounter = 1;
                
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

                Console.WriteLine($"Boat {boat.Name} has been buyed");
                
                myDataAPI.RemoveBoat(id);
                return true;
            }

        }
       



    }
}
