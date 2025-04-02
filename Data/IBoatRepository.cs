using System.Collections.ObjectModel;

namespace Data
{
    public abstract class IBoatRepository
    {
        public abstract ObservableCollection<IBoat> GetAllBoats();
        public abstract IBoat? GetBoatById(int id);
        public abstract void AddBoat(IBoat boat);
        public abstract void RemoveBoat(int id);
    }
}
