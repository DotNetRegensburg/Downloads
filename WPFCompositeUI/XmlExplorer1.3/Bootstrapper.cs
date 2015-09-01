using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.UnityExtensions;
using Microsoft.Practices.Unity;

namespace XmlExplorer
{
    class Bootstrapper : UnityBootstrapper
    {
        // Erzeuge MainWindow
        protected override DependencyObject CreateShell()
        {
            MainWindow shell = Container.Resolve<MainWindow>();
            shell.Show();
            return shell;
        }

        // Konfiguriere Dependency Injection-Container
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            
            // Bauplan erstellen...
            Container.RegisterType<
                IFileProvider, DialogFileProvider>();
            Container.RegisterType<
                IMainWindowPresentationModel, MainWindowPresentationModel>();
        }
    }
}
