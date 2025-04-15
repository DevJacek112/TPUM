namespace Data
{
    public abstract class IBoat
    {
        public abstract int Id { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract float Price { get; set; }
    }
}
