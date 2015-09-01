using System;
using System.Activities;
using System.Activities.DurableInstancing;
using System.IO;

namespace Articles.Workflow.WorkflowPersistence
{
    public static class Program
    {
        /// <summary>
        /// Main method of the application
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Console.WriteLine("Steps: ");
            Console.WriteLine(" 1 - Start the workflow and persist it");
            Console.WriteLine(" 2 - Reload the persisted workflow");
            Console.WriteLine();
            Console.Write(">> ");
            string stepToPerform = Console.ReadLine();

            //SqlWorkflowInstanceStore instanceStore = new SqlWorkflowInstanceStore("Data Source=IGZ-M94\\SQLEXPRESS;Initial Catalog=WF_PERSISTENCE;Integrated Security=True");
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "WFInstanceStore.zip");
            ZipWorkflowInstanceStore instanceStore = new ZipWorkflowInstanceStore(filePath);
            WorkflowApplication wfApplication = new WorkflowApplication(new MainActivity());
            wfApplication.InstanceStore = instanceStore;

            switch(stepToPerform)
            {
                case "1":
                    StartAndPersist(wfApplication);
                    break;

                case "2":
                    ReloadWorkflow(wfApplication);
                    break;
            }
        }

        /// <summary>
        /// Starts and persists the workflow.
        /// </summary>
        /// <param name="wfApplication">The WorkflowApplication object to use.</param>
        public static void StartAndPersist(WorkflowApplication wfApplication)
        {
            wfApplication.PersistableIdle = (eArgs) =>
            {
                Console.WriteLine("Workflow is idle => persist it!");
                Console.WriteLine("ID = " + eArgs.InstanceId.ToString());
                return PersistableIdleAction.Persist;
            };

            wfApplication.Run();
            wfApplication.Completed = (eArgs) => Console.WriteLine("Workflow completed!");

            Console.WriteLine("Press any key to unload the workflow");
            Console.ReadLine();

            wfApplication.Unload();
        }

        /// <summary>
        /// Reloads a persistet workflow from database.
        /// </summary>
        /// <param name="wfApplication">The WorkflowApplication object to use.</param>
        public static void ReloadWorkflow(WorkflowApplication wfApplication)
        {
            Console.Write("Workflow Instance ID: ");
            string id = Console.ReadLine();

            wfApplication.Load(new Guid(id));
            wfApplication.Run();

            Console.WriteLine("Workflow reloaded..");
            Console.ReadLine();
        }
    }
}
