namespace Model;

public class ModelBoat : IModelBoat
{
    public override int Id { get; }
    public override string Name { get; }
    public override string Description { get; }
    public override float Price { get; }

    public ModelBoat(int id, string name, string description, float price) 
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
    }
}