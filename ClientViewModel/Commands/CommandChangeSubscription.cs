using System.Windows.Input;
using Model;

namespace ViewModel;

public class CommandChangeSubscription : ICommand
{
    
    private readonly ClientAbstractModelAPI modelAPI;
    private readonly ClientViewModelAPI viewModelAPI;

    public CommandChangeSubscription(ClientAbstractModelAPI modelAPI, ClientViewModelAPI viewModelAPI)
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
        modelAPI.SetSubscription(viewModelAPI.IsNewsletterSubscribed);
    }

    public event EventHandler? CanExecuteChanged;
    
}