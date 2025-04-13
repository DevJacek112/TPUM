using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model;

namespace ViewModel;

public class ClientViewModelAPI : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public CommandBuyBoat BuyBoatCom { get; }
    public CommandSetPriceFilter SetPriceFilterCom { get; }
    public CommandChangeSubscriptions ChangeSubscriptionsCom { get; }

    public ObservableCollection<IModelBoat> ModelBoats => modelAPI.GetModelBoats();

    private ClientAbstractModelAPI modelAPI;
    private int actualTime;
    private int actualBoatsCount;
    private int actualClientsCount;

    private int minPrice;
    public int MinPrice
    {
        get => minPrice;
        set
        {
            minPrice = value;
            OnPropertyChanged();
        }
    }

    private int maxPrice;
    public int MaxPrice
    {
        get => maxPrice;
        set
        {
            maxPrice = value;
            OnPropertyChanged();
        }
    }

    private bool showTime;
    public bool ShowTime
    {
        get => showTime;
        set
        {
            if (showTime != value)
            {
                showTime = value;
                OnPropertyChanged();
            }
        }
    }

    private bool showBoatCount;
    public bool ShowBoatCount
    {
        get => showBoatCount;
        set
        {
            if (showBoatCount != value)
            {
                showBoatCount = value;
                OnPropertyChanged();
            }
        }
    }

    private bool showClientCount;
    public bool ShowClientCount
    {
        get => showClientCount;
        set
        {
            if (showClientCount != value)
            {
                showClientCount = value;
                OnPropertyChanged();
            }
        }
    }

    public int ActualTime
    {
        get => actualTime;
        set
        {
            actualTime = value;
            OnPropertyChanged();
        }
    }

    public int BoatsCount
    {
        get => actualBoatsCount;
        set
        {
            actualBoatsCount = value;
            OnPropertyChanged();
        }
    }

    public int ClientsCount
    {
        get => actualClientsCount;
        set
        {
            actualClientsCount = value;
            OnPropertyChanged();
        }
    }

    public ClientViewModelAPI()
    {
        modelAPI = ClientAbstractModelAPI.createInstance();
        BuyBoatCom = new CommandBuyBoat(modelAPI);
        SetPriceFilterCom = new CommandSetPriceFilter(modelAPI, this);
        ChangeSubscriptionsCom = new CommandChangeSubscriptions(modelAPI, this);

        modelAPI.actualTime.Subscribe(
            x => UpdateGUITimer(x),
            ex => Console.WriteLine($"Error: {ex.Message}"),
            () => Console.WriteLine("End of streaming.")
        );

        modelAPI.actualBoatCount.Subscribe(
            x => UpdateBoatsCount(x),
            ex => Console.WriteLine($"Error: {ex.Message}"),
            () => Console.WriteLine("End of streaming.")
        );

        modelAPI.actualClientsCount.Subscribe(
            x => UpdateClientsCount(x),
            ex => Console.WriteLine($"Error: {ex.Message}"),
            () => Console.WriteLine("End of streaming.")
        );
    }

    private void UpdateGUITimer(int time)
    {
        ActualTime = time;
    }

    private void UpdateBoatsCount(int boatsCount)
    {
        BoatsCount = boatsCount;
    }

    private void UpdateClientsCount(int clientsCount)
    {
        ClientsCount = clientsCount;
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
