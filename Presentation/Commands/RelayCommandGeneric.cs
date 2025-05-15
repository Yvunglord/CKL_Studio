using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CKL_Studio.Presentation.Commands
{
    public sealed class RelayCommand<T> : ICommand
    {
        private readonly Action<T?> _execute;
        private readonly Func<T?, bool>? _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecute == null)
                return true;

            if (parameter == null)
            {
                return _canExecute(default);
            }

            if (parameter is T typedParameter)
            {
                return _canExecute(typedParameter);
            }

            return false;
        }

        public void Execute(object? parameter)
        {
            if (parameter == null)
            {
                _execute(default);
                return;
            }

            if (parameter is T typedParameter)
            {
                _execute(typedParameter);
            }
            else
            {
                _execute(default);
            }
        }
    }
}
