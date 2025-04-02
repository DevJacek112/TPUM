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
    
    private AbstractModelAPI modelAPI;
    private int actualTime;

    public int ActualTime
    {
        get => actualTime;
        set
        {
            actualTime = value;
            OnPropertyChanged();
        }
    }
    
    public ViewModelAPI()
    {
        modelAPI = AbstractModelAPI.createInstance();
        BuyBoatCom = new CommandBuyBoat(modelAPI);
        modelAPI.OnTimePassedModel += UpdateGUITimer;
        ActualTime = 0;
    }

    private void UpdateGUITimer()
    {
        ActualTime++;
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}