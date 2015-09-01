using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Events;
using XmlExplorerShared;

namespace XmlExplorerModule
{
    public class ExplorerViewPresentationModel : IExplorerViewPresentationModel, INotifyPropertyChanged
    {
        private XmlNodeList nodes;

        public ExplorerViewPresentationModel(IEventAggregator eventAggregator, INodeListProvider nodeListProvider)
        {
            EventAggregator = eventAggregator;
            NodeListProvider = nodeListProvider;

            // Für Dateiänderungen in der Shell registrieren.
            FileChangedEvent fileChangedEvent = EventAggregator.GetEvent<FileChangedEvent>();
            // Leider sind wir nicht völlig vor Infrastuktur-Aspekten isoliert: 
            // In einem Unit-Test steht uns kein WPF-Thread-Dispatcher zur Verfügung.
            ThreadOption threadOption = 
                (Application.Current != null ? ThreadOption.UIThread : ThreadOption.PublisherThread);
            fileChangedEvent.Subscribe(OnFileChanged, threadOption);
        }

        internal IEventAggregator EventAggregator
        {
            get;
            private set;
        }

        internal INodeListProvider NodeListProvider
        {
            get;
            private set;
        }

        private void OnFileChanged(string fileName)
        {
            // Aktuelle Datei in der Shell hat sich geändert...
            if (String.IsNullOrEmpty(fileName))
            {
                // Datei wurde geschlossen.
                Nodes = null;
            }
            else
            {
                // Neue Knotenliste laden.
                Nodes = NodeListProvider.Select(fileName);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region IExplorerViewPresentationModel Members

        public XmlNodeList Nodes
        {
            get { return this.nodes; }
            private set
            {
                this.nodes = value;
                OnPropertyChanged("Nodes");
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
