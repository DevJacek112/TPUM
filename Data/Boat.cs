using System.Runtime.Serialization;

namespace Data
{
    internal class Boat : IBoat
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
    }
}
