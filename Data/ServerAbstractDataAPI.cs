using System.Collections.ObjectModel;

namespace Data
{
    public abstract class ServerAbstractDataAPI
    {
        public static ServerAbstractDataAPI createInstance()
        {
            return new ServerDataAPI();
        }

        public abstract ObservableCollection<IBoat> GetAllBoats();
        public abstract IBoat? GetBoatById(int id);
        public abstract void AddBoat(int id, string name, string description, float price);
        public abstract void RemoveBoat(int id);

        private class ServerDataAPI : ServerAbstractDataAPI
        {
            private readonly BoatRepository boats;
            private readonly IUser user;

            public ServerDataAPI()
            {
                boats = new BoatRepository();
                user = new User("Uzytkownik1", 150.0f);
            }

            public override ObservableCollection<IBoat> GetAllBoats()
            {
                return boats.GetAllBoats();
            }

            public override IBoat? GetBoatById(int id)
            {
                return boats.GetBoatById(id);
            }

            public override void AddBoat(int id, string name, string description, float price)
            {
                boats.AddBoat(new Boat(id, name, description, price));
            }

            public override void RemoveBoat(int id)
            {
                boats.RemoveBoat(id);
            }
        }
    }
}
