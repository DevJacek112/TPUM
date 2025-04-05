using System.Collections.ObjectModel;
using System.Timers;
using Timer = System.Threading.Timer;

namespace ClientLogic;

using ClientData;
    public abstract class ClientAbstractLogicAPI
    {
        public static ClientAbstractLogicAPI createInstance(ClientAbstractDataAPI? dataAPI = null)
        {
            return new ClientLogicAPI (dataAPI ?? ClientAbstractDataAPI.createInstance());
        }
        
        public abstract ObservableCollection<BoatDTO> GetAllBoats();
        public abstract void buyBoat(int id);
        private class ClientLogicAPI : ClientAbstractLogicAPI
        {
            private readonly ClientAbstractDataAPI myDataAPI;
            
            public ClientLogicAPI(ClientAbstractDataAPI? dataAPI)
            {
                myDataAPI = dataAPI;
            }

            public override ObservableCollection<BoatDTO> GetAllBoats()
            {
                return myDataAPI.GetAllBoats();
            }

            public override void buyBoat(int id)
            {
                myDataAPI.BuyBoatById(id);
            }
        }

    }