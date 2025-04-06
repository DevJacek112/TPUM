using System.Collections.ObjectModel;
using Data;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Logic
{
    public abstract class ServerAbstractLogicAPI
    {
        public static ServerAbstractLogicAPI createInstance(ServerAbstractDataAPI? dataAPI = null)
        {
            return new LogicApi(dataAPI ?? ServerAbstractDataAPI.createInstance());
        }

        public abstract ObservableCollection<IBoat> GetAllBoats();
        public abstract void addBoat(string name, string description, float price);
        public abstract bool buyBoat(int id);
        public abstract event Action OnTimePassed;
        public abstract void StartTimer();

        private static Timer timer;
        private static int secondsElapsed = 0;
        private class LogicApi : ServerAbstractLogicAPI
        {
            private readonly ServerAbstractDataAPI myDataAPI;
            private int IdCounter = 1;
            private Object padlock = new object();
            public override event Action OnTimePassed;
            
            public LogicApi(ServerAbstractDataAPI? dataAPI)
            {
                myDataAPI = dataAPI;
                IdCounter = dataAPI.GetAllBoats().Count + 1;
            }

            public override ObservableCollection<IBoat> GetAllBoats()
            {
                lock(padlock)
                {
                    return myDataAPI.GetAllBoats();
                }
            }

            public override void addBoat(string name, string description, float price)
            {
                lock(padlock)
                {
                    myDataAPI.AddBoat(IdCounter++, name, description, price);
                }
            }

            public override bool buyBoat(int id)
            {
                lock (padlock)
                {
                    var boat = myDataAPI.GetBoatById(id);
                    if (boat == null)
                    {
                        return false;
                    }
                    myDataAPI.RemoveBoat(id);
                    Console.WriteLine("    ");
                    return true;
                }
            }

            public override void StartTimer()
            {
                timer = new Timer(1000);
                timer.Elapsed += TimerElapsed;
                timer.AutoReset = true;
                timer.Start();
            }
            
            private void TimerElapsed(object sender, ElapsedEventArgs e)
            {
                secondsElapsed++;
                OnTimePassed?.Invoke();
            }
        }

    }
}
