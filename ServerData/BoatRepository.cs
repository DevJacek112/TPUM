using System.Collections.ObjectModel;

namespace Data
{
    internal class BoatRepository : IBoatRepository
    {
        public BoatRepository()
        {
            AddBoat(new Boat(1,"lodz1", "opis lodzi1", 21));
            AddBoat(new Boat(2,"lodz2", "opis lodzi2", 22));
            AddBoat(new Boat(3,"lodz3", "opis lodzi3", 23));
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
