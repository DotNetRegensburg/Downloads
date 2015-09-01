using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace XmlExplorer
{
    public class MainWindowPresentationModel : IMainWindowPresentationModel
    {
        public MainWindowPresentationModel()
        {
            // Konfiguriere Commands
            Open = new DelegateCommand<object>(
                OnOpenExecuted);
            Close = new DelegateCommand<object>(
                OnCloseExecuted, CanCloseExecute);
            Exit = new DelegateCommand<object>(
                OnExitExecuted);
        }

        #region IMainWindowPresentationModel Members

        public DelegateCommand<object> Open
        {
            get;
            private set;
        }

        public DelegateCommand<object> Exit
        {
            get;
            private set;
        }

        public DelegateCommand<object> Close
        {
            get;
            private set;
        }

        #endregion

        private void OnOpenExecuted(object parameter)
        {
        }

        private void OnCloseExecuted(object parameter)
        {
        }

        private bool CanCloseExecute(object parameter)
        {
            return false;
        }

        private void OnExitExecuted(object parameter)
        {
            Application.Current.Shutdown();
        }
    }
}
