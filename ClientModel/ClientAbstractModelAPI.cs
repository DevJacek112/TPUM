using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ClientLogic;

namespace Model
{
    public abstract class ClientAbstractModelAPI
    {
        public abstract ObservableCollection<IModelBoat> GetModelBoats();
        
        private ObservableCollection<IModelBoat> modelBoats = new ObservableCollection<IModelBoat>();
        
        public static ClientAbstractModelAPI createInstance(ClientAbstractLogicAPI? logicApi = null)
        {
            return new ModelAPI (logicApi ?? ClientAbstractLogicAPI.createInstance());
        }
        
        private Subject<string> newsletterSubject = new Subject<string>();
        public IObservable<string> newsletter => newsletterSubject.AsObservable();
        
        private Subject<int> actualTimeSubject = new Subject<int>();
        public IObservable<int> actualTime => actualTimeSubject.AsObservable();
        
        private Subject<int> actualBoatCountSubject = new Subject<int>();
        public IObservable<int> actualBoatCount => actualBoatCountSubject.AsObservable();
        
        private Subject<int> actualClientsCountSubject = new Subject<int>();
        public IObservable<int> actualClientsCount => actualClientsCountSubject.AsObservable();
        
        public abstract void BuyBoat(int id);
        
        public abstract void SetPriceFilter(float minPrice, float maxPrice);
        public abstract void SetFilters(bool serverTime, bool boatsCount, bool clientsCount);
        
        public abstract void SetSubscription(bool isNewsletterSubscribed);
        
        private class ModelAPI : ClientAbstractModelAPI
        {
            public ModelAPI(ClientAbstractLogicAPI? logicApi)
            {
                myLogicAPI = logicApi;
                myLogicAPI.GetAllBoats().CollectionChanged += CollectionUpdated;
                LoadAllBoats();
                
                myLogicAPI.newsletter
                    .Subscribe(
                        x => newsletterSubject.OnNext(x),
                        ex => Console.WriteLine($"Error: {ex.Message}"),
                        () => Console.WriteLine("End of streaming.")
                    );
                
                myLogicAPI.actualTime.Subscribe(
                    x =>actualTimeSubject.OnNext(x),  // onNext
                    ex => Console.WriteLine($"Error: {ex.Message}"),         // onError
                    () => Console.WriteLine("End of streaming.")           // onCompleted
                );
                
                myLogicAPI.actualBoatCount.Subscribe(
                    x =>actualBoatCountSubject.OnNext(x),  // onNext
                    ex => Console.WriteLine($"Error: {ex.Message}"),         // onError
                    () => Console.WriteLine("End of streaming.")           // onCompleted
                );
                
                myLogicAPI.actualClientsCount.Subscribe(
                    x =>actualClientsCountSubject.OnNext(x),  // onNext
                    ex => Console.WriteLine($"Error: {ex.Message}"),         // onError
                    () => Console.WriteLine("End of streaming.")           // onCompleted
                );
            }
            
            public override ObservableCollection<IModelBoat> GetModelBoats()
            {
                return modelBoats;
            }
            
            private ClientAbstractLogicAPI myLogicAPI;

            private void CollectionUpdated(object? sender, NotifyCollectionChangedEventArgs e)
            {
                LoadAllBoats();
            }

            private void LoadAllBoats()
            {
                modelBoats.Clear();
                var logicBoats = myLogicAPI.GetAllBoats();
                foreach (var boat in logicBoats)
                {
                    modelBoats.Add(new ModelBoat(boat.Id, boat.Name, boat.Description, boat.Price));
                }
            }

            public override void BuyBoat(int id)
            {
                myLogicAPI.BuyBoat(id);
            }

            public override void SetPriceFilter(float minPrice, float maxPrice)
            {
                myLogicAPI.SetPriceFilter(minPrice, maxPrice);
            }

            public override void SetFilters(bool serverTime, bool boatsCount, bool clientsCount)
            {
                myLogicAPI.SetFilters(serverTime, boatsCount, clientsCount);
            }
            
            public override void SetSubscription(bool isNewsletterSubscribed)
            {
                myLogicAPI.SetSubscription(isNewsletterSubscribed);
            }
        }
    }

}