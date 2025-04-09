namespace Model
{
    public abstract class IModelBoat
    {
        public IModelBoat CreateModelBoat(int id, string name, string description, float price)
        {
            return new ModelBoat(id, name, description, price);
        }
        
        public abstract int Id { get; }
        public abstract string Name { get; }
        public abstract float Price { get; }
        public abstract string Description { get; }
    } 
}
