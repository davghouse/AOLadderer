using System;
using System.Windows.Input;

namespace AOLadderer.UI
{
    // https://msdn.microsoft.com/en-us/magazine/dd419663.aspx
    public class RelayCommandParameterized<T> : ICommand
    {
        private readonly Func<T, bool> _canExecute;
        private readonly Action<T> _execute;

        public RelayCommandParameterized(Action<T> execute)
            : this(_ => true, execute)
        { }

        public RelayCommandParameterized(Func<T, bool> canExecute, Action<T> execute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
            => _canExecute((T)parameter);

        public void Execute(object parameter)
            => _execute((T)parameter);
    }
}
