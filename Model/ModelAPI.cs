using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Logic;

namespace Model
{
    public abstract class AbstractModelAPI
    {
        public abstract ObservableCollection<IModelBoat> GetModelBoats();
        
        public ObservableCollection<IModelBoat> modelBoats = new ObservableCollection<IModelBoat>();
        
        public static AbstractModelAPI createInstance(AbstractLogicAPI abstractLogicAPI = default)
        {
            return new ModelAPI(abstractLogicAPI);
        }
        
        public abstract void BuyBoat(int id);
        
        public abstract event Action OnTimePassedModel;
        
        internal class ModelAPI : AbstractModelAPI
        {
            public ModelAPI(AbstractLogicAPI abstractLogicAPI)
            {
                logicAPI = AbstractLogicAPI.createInstance();
                //temporary manually adding boats
                logicAPI.addBoat("lodz1", "opis lodzi1", 21);
                logicAPI.addBoat("lodz2", "opis lodzi2", 22);
                logicAPI.addBoat("lodz3", "opis lodzi3", 23);
                logicAPI.StartTimer();
                logicAPI.OnTimePassed += UpdateTimer;
                LoadAllBoats();
                GetModelBoats().CollectionChanged += OnModelBoatsChanged;
            }
            
            private void OnModelBoatsChanged(object? sender, NotifyCollectionChangedEventArgs e)
            {
                Console.WriteLine("OnModelBoatsChanged");
                LoadAllBoats();
            }
            
            public override ObservableCollection<IModelBoat> GetModelBoats()
            {
                return modelBoats;
            }
            
            private AbstractLogicAPI logicAPI;

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
                LoadAllBoats();
            }

            public override event Action OnTimePassedModel;
        }

    }

}