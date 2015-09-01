using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.UnityExtensions;
using Microsoft.Practices.Unity;
using System.IO;

namespace XmlExplorer
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            MainWindow shell = Container.Resolve<MainWindow>();
            shell.Show();
            return shell;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            // Bauplan erstellen...
            Container.RegisterType<
                IFileProvider, DialogFileProvider>();
            Container.RegisterType<
                IMainWindowPresentationModel, MainWindowPresentationModel>();
        }

        protected override IModuleCatalog GetModuleCatalog()
        {
            // Module aus Unterverzeichnis laden
            return new DirectoryModuleCatalog()
            {
                ModulePath = @".\Modules"
            };
        }
    }
}
