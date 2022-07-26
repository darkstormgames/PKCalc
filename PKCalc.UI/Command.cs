using System;
using System.Windows.Input;

namespace PKCalc.UI
{
    public class Command<T> : ICommand
    {
        public Command(Func<T?, bool>? canExecute = null, Action<T?>? execute = null)
        {
            this.CanExecuteDelegate = canExecute;
            this.ExecuteDelegate = execute;
        }

        public Func<T?, bool>? CanExecuteDelegate { get; }

        public Action<T?>? ExecuteDelegate { get; }

        public bool CanExecute(object? parameter)
        {
            var canExecute = this.CanExecuteDelegate;
            return canExecute is null || canExecute(parameter is T t ? t : default);
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object? parameter)
        {
            this.ExecuteDelegate?.Invoke(parameter is T t ? t : default);
        }
    }
}
