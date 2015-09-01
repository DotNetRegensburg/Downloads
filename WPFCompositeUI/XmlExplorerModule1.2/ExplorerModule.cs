using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Composite.Regions;
using XmlExplorerShared;

namespace XmlExplorerModule
{
    public class ExplorerModule : IModule
    {
        public ExplorerModule(IUnityContainer container)
        {
            Container = container;
        }

        internal IUnityContainer Container
        {
            get;
            private set;
        }

        #region IModule Members

        public void Initialize()
        {
            // Services registrieren
            Container.RegisterType<
                INodeListProvider, FileNodeListProvider>();
            Container.RegisterType<
                IExplorerViewPresentationModel, ExplorerViewPresentationModel>();

            // View anzeigen?
        }

        #endregion
    }
}
