using System.Collections.ObjectModel;

namespace Data
{
    internal class BoatRepository : IBoatRepository
    {
        public BoatRepository()
        {
            AddBoat(new Boat(1,"lodz1", "opis lodzi1", 100));
            AddBoat(new Boat(2,"lodz2", "opis lodzi2", 200));
            AddBoat(new Boat(3,"lodz3", "opis lodzi3", 300));
            AddBoat(new Boat(4,"lodz4", "opis lodzi4", 400));
            AddBoat(new Boat(5,"lodz5", "opis lodzi5", 500));
            AddBoat(new Boat(6,"lodz6", "opis lodzi6", 600));
            AddBoat(new Boat(7,"lodz7", "opis lodzi7", 700));
            AddBoat(new Boat(8,"lodz8", "opis lodzi8", 800));
            AddBoat(new Boat(9,"lodz9", "opis lodzi9", 900));
            AddBoat(new Boat(10,"lodz10", "opis lodzi10", 1000));
            AddBoat(new Boat(11,"lodz11", "opis lodzi11", 1100));
            AddBoat(new Boat(12,"lodz12", "opis lodzi12", 1200));
            AddBoat(new Boat(13,"lodz13", "opis lodzi13", 1300));
        }

        private ObservableCollection<IBoat> boats = new();

        public override ObservableCollection<IBoat> GetAllBoats()
        {
            return boats;
        }

        public override IBoat? GetBoatById(int id)
        {
            return boats.FirstOrDefault(b => b.Id == id);
        }

        public override void AddBoat(IBoat boat)
        {
            boats.Add(boat);
        }
        public override void RemoveBoat(int id)
        {
            var boat = boats.FirstOrDefault(b => b.Id == id);
            if (boat != null)
            {
                boats.Remove(boat);
            }
        }



    }
}
