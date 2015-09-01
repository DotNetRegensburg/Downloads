using System;
using System.Activities;
using System.Activities.XamlIntegration;
using System.IO;
using System.Linq;
using System.ServiceModel.Activities;
using System.ServiceModel.Description;
using System.Threading;
using System.Windows.Forms;

namespace Articles.Workflow.CustomWorkflowService
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Just a dummy to trigger loading of an assembly
            System.ServiceModel.Activities.Receive receiveTask = null;

            //Initialize application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += OnApplicationThreadException;

            if ((args != null) && (args.Contains("-runserver")))
            {
                try
                {
                    //Get commandline arguments
                    OwnerWindowWrapper windowWrapper = new OwnerWindowWrapper(
                        new IntPtr(Int32.Parse(args[2])));
                    string workflowFilePath = args[1];

                    //Load workflow
                    Activity activity = null;
                    WorkflowServiceHost serviceHost = null;
                    ProgressDialog.Show(windowWrapper, () =>
                    {
                        using (FileStream inStream = File.OpenRead(workflowFilePath))
                        {
                            activity = ActivityXamlServices.Load(inStream);
                        }

                        string baseAddress = "http://localhost:5020/CustomService";
                        serviceHost = new WorkflowServiceHost(activity, new Uri(baseAddress));
                        serviceHost.Description.Behaviors.Add(new ServiceMetadataBehavior() 
                        { 
                            HttpGetEnabled = true 
                        });
                        serviceHost.Open();
                    });

                    //Create and show the server window
                    ServerHostWindow serverHostWindow = new ServerHostWindow(serviceHost, activity);
                    serverHostWindow.Show(windowWrapper);
                    Application.Run(serverHostWindow);

                    serviceHost.Close();
                }
                catch (Exception ex)
                {
                    ShowException(ex);
                }
            }
            else
            {
                //Run default editor application
                Application.Run(new MainWindow());
            }
        }

        /// <summary>
        /// Called when an unhandled exception occurred.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Threading.ThreadExceptionEventArgs"/> instance containing the event data.</param>
        private static void OnApplicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            ShowException(e.Exception);
        }

        /// <summary>
        /// Shows the given exception.
        /// </summary>
        /// <param name="ex">The exception to be shown.</param>
        private static void ShowException(Exception ex)
        {
            MessageBox.Show(
                ex.ToString(),
                "Error!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
