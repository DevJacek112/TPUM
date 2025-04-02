using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    internal class BoatRepository : IBoatRepository
    {
        private List<IBoat> boats = new();

        public override List<IBoat> GetAllBoats()
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
