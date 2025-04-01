using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public abstract class IBoatRepository
    {
        public abstract List<IBoat> GetAllBoats();
        public abstract IBoat? GetBoatById(int id);
        public abstract void AddBoat(IBoat boat);
        public abstract void RemoveBoat(int id);
    }
}
