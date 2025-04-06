using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
        
        private Subject<int> actualTimeSubject = new Subject<int>();
        public IObservable<int> actualTime => actualTimeSubject.AsObservable();
        private class ClientLogicAPI : ClientAbstractLogicAPI
        {
            private readonly ClientAbstractDataAPI myDataAPI;
            
            public ClientLogicAPI(ClientAbstractDataAPI? dataAPI)
            {
                myDataAPI = dataAPI;
                
                myDataAPI.actualTime.Subscribe(
                    x =>actualTimeSubject.OnNext(x),  // onNext
                    ex => Console.WriteLine($"Error: {ex.Message}"),         // onError
                    () => Console.WriteLine("End of streaming.")           // onCompleted
                );
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