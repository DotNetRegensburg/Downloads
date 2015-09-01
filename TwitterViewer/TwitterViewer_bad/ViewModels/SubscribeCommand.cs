using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace TwitterViewer.ViewModels
{
    class BaseCommand : ICommand
    {
        Action<object> _handler;
        Func<object, bool> _condition;

        public BaseCommand(Action<object> handler, Func<object, bool> condition = null)
        {
            _handler = handler;
            _condition = condition;
            // TODO: Complete member initialization
            //this.twitterMonitorViewModel = twitterMonitorViewModel;
            //this.twitterMonitorViewModel.PropertyChanged += (s, e) =>
            //{
            //    if (e.PropertyName == "SearchPattern" && CanExecuteChanged != null)
            //    {
            //        CanExecuteChanged(this, null);
            //    }
            //};
        }

        public bool CanExecute(object parameter)
        {
            return _condition != null ? _condition(parameter) : true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _handler(parameter);
        }
    }
}
