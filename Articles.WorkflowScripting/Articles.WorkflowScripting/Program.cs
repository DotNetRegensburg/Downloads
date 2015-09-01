using System;
using System.Diagnostics;
using System.Windows.Forms;
using Common.GraphicsEngine.Core;

namespace Articles.WorkflowScripting
{
    static class Program
    {
        private static MainWindow s_mainWindow;

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Initialize Application object
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           
            //Initialize graphics
            GraphicsCore.Initialize(TargetHardware.DirectX9, Debugger.IsAttached);

            //Show main window
            s_mainWindow = new MainWindow();
            Application.Run(s_mainWindow);
        }
    }
}
