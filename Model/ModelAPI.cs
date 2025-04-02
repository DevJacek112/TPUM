using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Logic;
namespace Model
{
    public abstract class AbstractModelAPI
    {
        public abstract ObservableCollection<IModelBoat> GetModelBoats();
        
        private ObservableCollection<IModelBoat> modelBoats = new ObservableCollection<IModelBoat>();
        
        public static AbstractModelAPI createInstance(AbstractLogicAPI abstractLogicAPI = default)
        {
            return new ModelAPI();
        }
        
        public abstract void BuyBoat(int id);
        
        public abstract event Action OnTimePassedModel;
        
        internal class ModelAPI : AbstractModelAPI
        {
            public override ObservableCollection<IModelBoat> GetModelBoats()
            {
                return modelBoats;
            }
            
            private AbstractLogicAPI logicAPI;
            public ModelAPI()
            {
                logicAPI = AbstractLogicAPI.createInstance();
                logicAPI.StartTimer();
                logicAPI.OnTimePassed += UpdateTimer;
                logicAPI.GetAllBoats().CollectionChanged += CollectionUpdated;
                LoadAllBoats();
            }

            private void CollectionUpdated(object? sender, NotifyCollectionChangedEventArgs e)
            {
                LoadAllBoats();
            }
            
            public void UpdateTimer()
            {
                OnTimePassedModel?.Invoke();
            }

            public void LoadAllBoats()
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

            public override event Action OnTimePassedModel;
        }

    }

}