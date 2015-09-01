using System;
using System.Activities;
using System.ServiceModel.Activities;
using System.ServiceModel.Description;
using System.Threading;

namespace Articles.Workflows.Hosting
{
    public class Program
    {
        /// <summary>
        /// Main method.
        /// </summary>
        public static void Main()
        {
            Console.WriteLine("Samples: ");
            Console.WriteLine(" 1 - Hosting using WorkflowInvoker");
            Console.WriteLine(" 2 - Hosting using WorkflowApplication");
            Console.WriteLine(" 3 - Hosting using WorkflowService");
            Console.WriteLine();

            Console.Write(">> ");
            switch(Console.ReadLine())
            {
                case "1":
                    HostUsingWorkflowInvoker();
                    break;

                case "2":
                    HostUsingWorkflowApplication();
                    break;

                case "3":
                    HostUsingWorkflowService();
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("Sample finished..");
            Console.ReadLine();
        }

        /// <summary>
        /// Hosting sample using WorkflowInvoker.
        /// </summary>
        public static void HostUsingWorkflowInvoker()
        {
            MainActivity mainActivity = new MainActivity();

            Console.WriteLine("A single call to WorkflowInvoker using the generated Activity");
            WorkflowInvoker.Invoke(mainActivity);
            Console.WriteLine("");

            Console.WriteLine("Multiple calls to WorkflowInvoker");
            WorkflowInvoker.Invoke(mainActivity);
            WorkflowInvoker.Invoke(mainActivity);
            WorkflowInvoker.Invoke(mainActivity);
            Console.WriteLine("");

            Console.WriteLine("Multiple calls to one WorkflowInvoker object");
            WorkflowInvoker wfInvoker = new WorkflowInvoker(mainActivity);
            wfInvoker.Invoke();
            wfInvoker.Invoke();
            wfInvoker.Invoke();
            Console.WriteLine("");

            Console.WriteLine("Multiple calls to one WorkflowInvoker object");
            WorkflowInvoker wfInvokerAsync = new WorkflowInvoker(mainActivity);
            wfInvokerAsync.InvokeAsync();
            wfInvokerAsync.InvokeAsync();
            wfInvokerAsync.InvokeAsync();
            Console.WriteLine("");
        }

        /// <summary>
        /// Hosting sample using WorkflowApplication.
        /// </summary>
        public static void HostUsingWorkflowApplication()
        {
            //Create workflow
            MainActivity mainActivity = new MainActivity();
            WorkflowApplication wfApplication = new WorkflowApplication(mainActivity);

            //Create simple synchronization mechanism
            EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            wfApplication.Completed = (eArgs) => waitHandle.Set();
            
            //Run workow and wait until finished
            wfApplication.Run();

            waitHandle.WaitOne();
        }

        /// <summary>
        /// Hosting sample using WorkflowService.
        /// </summary>
        public static void HostUsingWorkflowService()
        {
            Console.WriteLine("Starting service host..");

            string baseAddress = "http://localhost:5020/CustomService";
            WorkflowServiceHost serviceHost = new WorkflowServiceHost(
                new ServiceMainActivity(), 
                new Uri(baseAddress));
            serviceHost.Description.Behaviors.Add(new ServiceMetadataBehavior()
            {
                HttpGetEnabled = true
            });
            serviceHost.Open();

            Console.WriteLine("Started. Available on adress:");
            Console.WriteLine(baseAddress);
            Console.WriteLine();

            //Start an endless loop..
            while (true)
            {
                Thread.Sleep(500);
            }
        }
    }
}
