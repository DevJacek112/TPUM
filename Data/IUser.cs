using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public abstract class IUser
    {
        public abstract string Name { get; }
        public abstract float Money { get; set; }
    }
}
