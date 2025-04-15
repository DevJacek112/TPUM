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
        public abstract void BuyBoat(int id);
        
        public abstract void SetPriceFilter(float minPrice, float maxPrice);
        
        
        private Subject<int> actualTimeSubject = new Subject<int>();
        public IObservable<int> actualTime => actualTimeSubject.AsObservable();
        
        private Subject<string> newsletterSubject = new Subject<string>();
        public IObservable<string> newsletter => newsletterSubject.AsObservable();
        
        private Subject<int> actualBoatCountSubject = new Subject<int>();
        public IObservable<int> actualBoatCount => actualBoatCountSubject.AsObservable();
        
        private Subject<int> actualClientsCountSubject = new Subject<int>();
        public IObservable<int> actualClientsCount => actualClientsCountSubject.AsObservable();
        
        public abstract void SetSubsciptions(bool showServerTime, bool showBoatsCount, bool showClientsCount);
        
        private class ClientLogicAPI : ClientAbstractLogicAPI
        {
            private readonly ClientAbstractDataAPI myDataAPI;
            
            private BehaviorSubject<bool> showTimeSubject = new(false);
            private BehaviorSubject<bool> showBoatCountSubject = new(false);
            private BehaviorSubject<bool> showClientCountSubject = new(false);
            
            public ClientLogicAPI(ClientAbstractDataAPI? dataAPI)
            {
                myDataAPI = dataAPI;
                
                myDataAPI.newsletter
                    .Subscribe(
                        x => newsletterSubject.OnNext(x),
                        ex => Console.WriteLine($"Error: {ex.Message}"),
                        () => Console.WriteLine("End of streaming.")
                    );
                
                myDataAPI.actualTime
                    .WithLatestFrom(showTimeSubject, (time, show) => new { time, show })
                    .Where(x => x.show)
                    .Select(x => x.time)
                    .Subscribe(
                        x => actualTimeSubject.OnNext(x),
                        ex => Console.WriteLine($"Error: {ex.Message}"),
                        () => Console.WriteLine("End of streaming.")
                    );
                
                myDataAPI.actualBoatCount
                    .WithLatestFrom(showBoatCountSubject, (count, show) => new { count, show })
                    .Where(x => x.show)
                    .Select(x => x.count)
                    .Subscribe(x => actualBoatCountSubject.OnNext(x));

                myDataAPI.actualClientsCount
                    .WithLatestFrom(showClientCountSubject, (count, show) => new { count, show })
                    .Where(x => x.show)
                    .Select(x => x.count)
                    .Subscribe(x => actualClientsCountSubject.OnNext(x));
            }

            public override ObservableCollection<BoatDTO> GetAllBoats()
            {
                return myDataAPI.GetAllBoats();
            }

            public override void BuyBoat(int id)
            {
                myDataAPI.BuyBoatById(id);
            }

            public override void SetPriceFilter(float minPrice, float maxPrice)
            {
                myDataAPI.SetPriceFilter(minPrice, maxPrice);
            }

            public override void SetSubsciptions(bool showServerTime, bool showBoatsCount, bool showClientsCount)
            {
                showTimeSubject.OnNext(showServerTime);
                showBoatCountSubject.OnNext(showBoatsCount);
                showClientCountSubject.OnNext(showClientsCount);
            }
        }
    }