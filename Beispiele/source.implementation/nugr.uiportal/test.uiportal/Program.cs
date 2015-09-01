using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using nugr.uiportal;

namespace test.uiportal
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            MockupApplication app = new MockupApplication();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new WinMain(app));
        }
    }
}
