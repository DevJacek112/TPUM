using System.Windows.Input;
using Model;

namespace ViewModel;

public class CommandSetPriceFilter : ICommand
{
    private readonly ClientAbstractModelAPI modelAPI;
    private readonly ClientViewModelAPI viewModelAPI;

    public CommandSetPriceFilter(ClientAbstractModelAPI modelAPI, ClientViewModelAPI viewModelAPI)
    {
        this.modelAPI = modelAPI ?? throw new ArgumentNullException(nameof(modelAPI));
        this.viewModelAPI = viewModelAPI ?? throw new ArgumentNullException(nameof(viewModelAPI));
    }
    
    public event EventHandler? CanExecuteChanged;
    
    public bool CanExecute(object? parameter)
    {
        return viewModelAPI.MinPrice >= 0 && viewModelAPI.MaxPrice >= viewModelAPI.MinPrice;
    }

    public void Execute(object? parameter)
    {
        modelAPI.SetPriceFilter(viewModelAPI.MinPrice, viewModelAPI.MaxPrice);
    }
}