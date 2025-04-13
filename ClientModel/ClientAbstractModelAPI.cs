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
        
        private Subject<int> actualTimeSubject = new Subject<int>();
        public IObservable<int> actualTime => actualTimeSubject.AsObservable();
        
        public abstract void BuyBoat(int id);
        
        public abstract void SetPriceFilter(float minPrice, float maxPrice);
        
        private class ModelAPI : ClientAbstractModelAPI
        {
            public ModelAPI(ClientAbstractLogicAPI? logicApi)
            {
                myLogicAPI = logicApi;
                myLogicAPI.GetAllBoats().CollectionChanged += CollectionUpdated;
                LoadAllBoats();
                
                myLogicAPI.actualTime.Subscribe(
                    x =>actualTimeSubject.OnNext(x),  // onNext
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
        }

    }

}