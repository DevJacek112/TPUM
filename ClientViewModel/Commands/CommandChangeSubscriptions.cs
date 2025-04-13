using System.Windows.Input;
using Model;

namespace ViewModel;

public class CommandChangeSubscriptions : ICommand
{
    
    private readonly ClientAbstractModelAPI modelAPI;
    private readonly ClientViewModelAPI viewModelAPI;

    public CommandChangeSubscriptions(ClientAbstractModelAPI modelAPI, ClientViewModelAPI viewModelAPI)
    {
        this.modelAPI = modelAPI ?? throw new ArgumentNullException(nameof(modelAPI));
        this.viewModelAPI = viewModelAPI ?? throw new ArgumentNullException(nameof(viewModelAPI));
    }
    
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        modelAPI.SetSubsciptions(viewModelAPI.ShowTime, viewModelAPI.ShowBoatCount, viewModelAPI.ShowClientCount);
    }

    public event EventHandler? CanExecuteChanged;
}