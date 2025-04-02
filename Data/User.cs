using System.Runtime.Serialization;

namespace Data
{
    public class User : IUser, ISerializable
    {
        public override string Name { get; }
        public override float Money { get; set; }

        public User(string name, float money) 
        {
            Name = name;
            Money = money;
        }

        protected User(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("Name") ?? "Unknown";
            Money = info.GetSingle("Money");
        }

        // Serialization method
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("Money", Money);
        }
    }
}
