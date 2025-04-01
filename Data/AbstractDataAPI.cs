using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public abstract class AbstractDataAPI
    {
        public static AbstractDataAPI createInstance()
        {
            return new DataAPI();
        }

        public abstract List<IBoat> GetAllBoats();
        public abstract IBoat? GetBoatById(int id);
        public abstract void AddBoat(int id, string name, string description, float price);
        public abstract void RemoveBoat(int id);

        private class DataAPI : AbstractDataAPI
        {
            private readonly BoatRepository boats;
            
            public DataAPI()
            {
                boats = new BoatRepository();
            }

            public override List<IBoat> GetAllBoats()
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
