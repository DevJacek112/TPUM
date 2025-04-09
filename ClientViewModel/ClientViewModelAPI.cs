using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model;

namespace ViewModel;

public class ClientViewModelAPI : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public CommandBuyBoat BuyBoatCom { get; }
    
    public ObservableCollection<IModelBoat> ModelBoats => modelAPI.GetModelBoats();
    
    private ClientAbstractModelAPI modelAPI;
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
    
    public ClientViewModelAPI()
    {
        modelAPI = ClientAbstractModelAPI.createInstance();
        BuyBoatCom = new CommandBuyBoat(modelAPI);
        ActualTime = 0;
        
        modelAPI.actualTime.Subscribe(
            x =>UpdateGUITimer(x),  // onNext
            ex => Console.WriteLine($"Error: {ex.Message}"),         // onError
            () => Console.WriteLine("End of streaming.")           // onCompleted
        );
    }

    private void UpdateGUITimer(int time)
    {
        ActualTime = time;
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}