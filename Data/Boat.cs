using System.Runtime.Serialization;

namespace Data
{
    internal class Boat : IBoat, ISerializable
    {
        public override int Id { get; }
        public override string Name { get; }
        public override string Description { get; }
        public override float Price { get; set; }

        public Boat(int id, string name, string description, float price) 
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }

        protected Boat(SerializationInfo info, StreamingContext context)
        {
            Id = info.GetInt32("Id");
            Name = info.GetString("Name") ?? "Unknown";
            Description = info.GetString("Description") ?? "No Description";
            Price = info.GetSingle("Price");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID: ", Id);
            info.AddValue("Name: ", Name);
            info.AddValue("Description:", Description);
            info.AddValue("Price: ", Price);
        }
    }
}
