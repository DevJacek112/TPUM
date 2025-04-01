using System;
using System.Windows.Input;
using Model;

namespace ViewModel
{
    public class CommandBuyBoat : ICommand
    {
        private readonly AbstractModelAPI modelAPI;
        public event EventHandler? CanExecuteChanged;
    
        public CommandBuyBoat(AbstractModelAPI modelAPI)
        {
            this.modelAPI = modelAPI ?? throw new ArgumentNullException(nameof(modelAPI));
        }

        public bool CanExecute(object? parameter) => parameter is IModelBoat;

        public void Execute(object? parameter)
        {
            if (parameter is IModelBoat boat)
            {
                //Console.WriteLine($"executed, buying boat {boat.Id}");
                modelAPI.BuyBoat(boat.Id);
            }
            else
            {
                //Console.WriteLine("Invalid parameter passed to Execute.");
            }
        }
    }

}