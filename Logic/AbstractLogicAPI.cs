using Data;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Logic
{
    public abstract class AbstractLogicAPI
    {
        public static AbstractLogicAPI createInstance(AbstractDataAPI? dataAPI = null)
        {
            return new LogicAPI(dataAPI ?? AbstractDataAPI.createInstance());
        }

        public abstract List<IBoat> GetAllBoats();
        public abstract void addBoat(string name, string description, float price);
        public abstract bool buyBoat(int id);
        public abstract event Action OnTimePassed;
        public abstract void StartTimer();

        private static Timer timer;
        private static int secondsElapsed = 0;
        private class LogicAPI : AbstractLogicAPI
        {
            private readonly AbstractDataAPI myDataAPI;
            private int IdCounter = 1;
            public override event Action OnTimePassed;

            public LogicAPI(AbstractDataAPI? dataAPI)
            {
                myDataAPI = dataAPI;
                IdCounter = dataAPI.GetAllBoats().Count + 1;
            }

            public override List<IBoat> GetAllBoats()
            {
                return myDataAPI.GetAllBoats();
            }

            public override void addBoat(string name, string description, float price)
            {
                myDataAPI.AddBoat(IdCounter++, name, description, price);
            }

            public override bool buyBoat(int id)
            {
                var boat = myDataAPI.GetBoatById(id);
                if (boat == null)
                {
                    return false;
                }

                Console.WriteLine($"Boat {boat.Name} has been bought");
                
                myDataAPI.RemoveBoat(id);
                return true;
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
                
                if (secondsElapsed > 5)
                {
                    Console.WriteLine("Logika zaktualizowala cene");
                    GetAllBoats()[1].Price--;
                }
            }
        }

    }
}
