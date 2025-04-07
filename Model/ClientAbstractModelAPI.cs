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
        
        private class ModelAPI : ClientAbstractModelAPI
        {
            public ModelAPI(ClientAbstractLogicAPI? logicApi)
            {
                logicAPI = ClientAbstractLogicAPI.createInstance();
                logicAPI.GetAllBoats().CollectionChanged += CollectionUpdated;
                LoadAllBoats();
                
                logicAPI.actualTime.Subscribe(
                    x =>actualTimeSubject.OnNext(x),  // onNext
                    ex => Console.WriteLine($"Error: {ex.Message}"),         // onError
                    () => Console.WriteLine("End of streaming.")           // onCompleted
                );
            }
            
            public override ObservableCollection<IModelBoat> GetModelBoats()
            {
                return modelBoats;
            }
            
            private ClientAbstractLogicAPI logicAPI;

            private void CollectionUpdated(object? sender, NotifyCollectionChangedEventArgs e)
            {
                LoadAllBoats();
            }

            private void LoadAllBoats()
            {
                modelBoats.Clear();
                var logicBoats = logicAPI.GetAllBoats();
                foreach (var boat in logicBoats)
                {
                    modelBoats.Add(new ModelBoat(boat.Id, boat.Name, boat.Description, boat.Price));
                }
            }

            public override void BuyBoat(int id)
            {
                logicAPI.buyBoat(id);
            }
        }

    }

}