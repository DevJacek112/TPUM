using System.Runtime.Serialization;

namespace Data
{
    public abstract class IBoat : ISerializable
    {
        public abstract int Id { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract float Price { get; set; }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID: ", Id);
            info.AddValue("Name: ", Name);
            info.AddValue("Description:", Description);
            info.AddValue("Price: ", Price);
        }


    }
}
