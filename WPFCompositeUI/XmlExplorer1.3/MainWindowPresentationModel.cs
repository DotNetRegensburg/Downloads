using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Unity;
using XmlExplorerShared;

namespace XmlExplorer
{
    public class MainWindowPresentationModel : IMainWindowPresentationModel
    {
        private string currentFile;

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

        public string CurrentFileName
        {
            get
            {
                return this.currentFile;
            }
            set
            {
                this.currentFile = value;
                FileChangedEvent fileChangedEvent = 
                    EventAggregator.GetEvent<FileChangedEvent>();
                fileChangedEvent.Publish(value);
            }
        }

        [Dependency]
        public IEventAggregator EventAggregator
        {
            get;
            set;
        }

        [Dependency]
        public IFileProvider FileProvider
        {
            get;
            set;
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
            // Keine gute Idee... sonst öffnet sich beim Unit Test
            // ein Dialogfenster!
            //OpenFileDialog dialog = new OpenFileDialog()
            //{
            //    Filter = "XML Files|*.xml|All Files|*.*"
            //};
            //if (dialog.ShowDialog() == true)
            //{
            //}

            // Also bemühen wir einen Service
            string fileName = FileProvider.Select("XML Files", "*.xml");
            if (!String.IsNullOrEmpty(fileName))
            {
                CurrentFileName = fileName;
            }
        }

        private void OnCloseExecuted(object parameter)
        {
            CurrentFileName = null;
        }

        private bool CanCloseExecute(object parameter)
        {
            return !String.IsNullOrEmpty(CurrentFileName);
        }

        private void OnExitExecuted(object parameter)
        {
            Application.Current.Shutdown();
        }
    }
}
