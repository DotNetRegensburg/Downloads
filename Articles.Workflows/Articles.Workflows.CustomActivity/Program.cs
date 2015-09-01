
using System;
using System.Activities;

namespace Articles.Workflows.CustomActivity
{
    public class Program
    {
        /// <summary>
        /// Main method.
        /// </summary>
        public static void Main(string[] args)
        {
            //Dictionary<string, object> arguments = new Dictionary<string, object>();
            //arguments["SleepTime"] = 1500;
            //WorkflowInvoker.Invoke(
            //    new SampleSleepActivity(),
            //    arguments);

            WorkflowInvoker.Invoke(new MainActivity());

            Console.WriteLine("Sample finished..");
            Console.ReadLine();
        }
    }
}
