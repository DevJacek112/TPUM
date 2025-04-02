using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IUser
    {
        public abstract string Name { get; }
        public abstract float Money { get; set; }
        public abstract List<IBoat> myBoats;
    }
}
