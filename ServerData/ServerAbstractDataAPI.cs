using System.Collections.ObjectModel;
using Data.Newsletter;

namespace Data
{
    public abstract class ServerAbstractDataAPI
    {
        public static ServerAbstractDataAPI createInstance()
        {
            return new ServerDataAPI();
        }

        public abstract ObservableCollection<IBoat> GetAllBoats();
        public abstract IBoat? GetBoatById(int id);
        public abstract void AddBoat(int id, string name, string description, float price);
        public abstract void RemoveBoat(int id);
        
        public abstract ObservableCollection<INewsletterMessage> GetAllMessages();
        public abstract INewsletterMessage? GetNewMessageById(int id);

        private class ServerDataAPI : ServerAbstractDataAPI
        {
            private readonly BoatRepository boats;
            private readonly NewsletterRepository newsletter;

            public ServerDataAPI()
            {
                boats = new BoatRepository();
                newsletter = new NewsletterRepository();
            }

            public override ObservableCollection<IBoat> GetAllBoats()
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

            public override ObservableCollection<INewsletterMessage> GetAllMessages()
            {
                return newsletter.GetAllMessages();
            }

            public override INewsletterMessage? GetNewMessageById(int id)
            {
                return newsletter.GetMessageById(id);
            }
        }
    }
}
