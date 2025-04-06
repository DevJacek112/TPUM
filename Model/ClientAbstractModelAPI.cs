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
        
        public static ClientAbstractModelAPI createInstance()
        {
            return new ModelAPI();
        }
        
        private Subject<int> actualTimeSubject = new Subject<int>();
        public IObservable<int> actualTime => actualTimeSubject.AsObservable();
        
        public abstract void BuyBoat(int id);
        
        //public abstract event Action OnTimePassedModel;
        
        private class ModelAPI : ClientAbstractModelAPI
        {
            public ModelAPI()
            {
                logicAPI = ClientAbstractLogicAPI.createInstance();
                //logicAPI.StartTimer();
                //logicAPI.OnTimePassed += UpdateTimer;
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
            
            /*public void UpdateTimer()
            {
                OnTimePassedModel?.Invoke();
            }*/

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

            //public override event Action OnTimePassedModel;
        }

    }

}