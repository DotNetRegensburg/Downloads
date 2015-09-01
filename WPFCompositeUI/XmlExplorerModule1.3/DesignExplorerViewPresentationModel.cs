using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.ComponentModel;

namespace XmlExplorerModule
{
    public class DesignExplorerViewPresentationModel : IExplorerViewPresentationModel, INotifyPropertyChanged
    {
        private XmlNodeList nodes;
        
        private const string DesignTimeXml = @"
            <Family Name='Test Family'>
              <Person Name='Tom' Age='9' />
              <Person Name='John' Age='11' />
              <Person Name='Melissa' Age='36' />
              <Pet Name='Kitty' Animal='Cat' />
            </Family>
            ";


        public DesignExplorerViewPresentationModel()
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(DesignTimeXml);
            Nodes = document.SelectNodes("/");
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
