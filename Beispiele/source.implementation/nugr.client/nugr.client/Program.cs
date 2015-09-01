using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using Nug.DataAdapter;
using nugr.application;
using nugr.calculator;
using nugr.contract.application;
using nugr.contract.calculator;
using nugr.contract.dbadapter;
using nugr.uiportal;
using ralfw.ComponentDependencies;
using ralfw.ComponentDependencies.Contract;
using ralfw.ComponentDependencies.DIContainerMappers;

namespace nugr.client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IDependencyGraph dg = new DependencyGraph("PrimeCalc");
            dg.AddRoot(
                new Component<Form, WinMain>(
                    new Component<IApplication, PrimeApp>(
                        new Component<ICalculator, PrimeCalc>(),
                        new Component<IDBAdapter, DataAdapter>()
                        )
                    )
                );

            if (Environment.CommandLine.ToLower().IndexOf("/dependencydiagram") > 0)
            {
                dg.ToBitmap().Save("nugr.dependencydiagram.jpg", ImageFormat.Jpeg);
                return;
            }

            // code.google.com/componentdependencygraph
            IUnityContainer uc;
            uc = dg.MapTo<IUnityContainer>(new UnityMapper());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(uc.Resolve<Form>());
        }
    }
}
