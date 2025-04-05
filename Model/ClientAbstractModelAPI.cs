using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ClientLogic;
using Logic;
namespace Model
{
    public abstract class ClientAbstractModelAPI
    {
        public abstract ObservableCollection<IModelBoat> GetModelBoats();
        
        private ObservableCollection<IModelBoat> modelBoats = new ObservableCollection<IModelBoat>();
        
        public static ClientAbstractModelAPI createInstance(AbstractServerLogicAPI abstractServerLogicApi = default)
        {
            return new ModelAPI();
        }
        
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