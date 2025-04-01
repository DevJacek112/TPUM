using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model;

namespace ViewModel;

public class ViewModelAPI : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public CommandBuyBoat BuyBoatCom { get; }
    
    public ObservableCollection<IModelBoat> ModelBoats => modelAPI.GetModelBoats();
    
    AbstractModelAPI modelAPI;
    
    public ViewModelAPI()
    {
        modelAPI = AbstractModelAPI.createInstance();
        BuyBoatCom = new CommandBuyBoat(modelAPI);
    }
}