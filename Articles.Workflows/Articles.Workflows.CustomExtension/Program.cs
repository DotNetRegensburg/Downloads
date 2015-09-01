using System;
using System.Threading;
using System.Windows.Forms;

namespace Articles.Workflows.CustomExtension
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(OnApplicationThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }

        /// <summary>
        /// Called for each unhandled exception in gui thread.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Threading.ThreadExceptionEventArgs"/> instance containing the event data.</param>
        private static void OnApplicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(
                e.Exception.ToString(),
                "Error!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
