using System.Collections.ObjectModel;
using Logic;

namespace Model
{
    public abstract class AbstractModelAPI
    {
        public abstract ObservableCollection<IModelBoat> GetModelBoats();
        
        private ObservableCollection<IModelBoat> modelBoats = new ObservableCollection<IModelBoat>();
        
        public static AbstractModelAPI createInstance(AbstractLogicAPI abstractLogicAPI = default)
        {
            return new ModelAPI(abstractLogicAPI);
        }
        
        public abstract void BuyBoat(int id);
        
        internal class ModelAPI : AbstractModelAPI
        {
            public override ObservableCollection<IModelBoat> GetModelBoats()
            {
                return modelBoats;
            }
            
            private AbstractLogicAPI logicAPI;
            public ModelAPI(AbstractLogicAPI abstractLogicAPI)
            {
                logicAPI = AbstractLogicAPI.createInstance();
                //temporary manually adding boats
                logicAPI.addBoat("lodz1", "opis lodzi1", 21);
                logicAPI.addBoat("lodz2", "opis lodzi2", 22);
                logicAPI.addBoat("lodz3", "opis lodzi3", 23);
                LoadAllBoats();
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
        }

    }

}