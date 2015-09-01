using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activities;
using System.ServiceModel.Description;

namespace Articles.Workflows.BusinessCase
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting service host..");

            string baseAddress = "http://localhost:5020/CustomService";
            WorkflowServiceHost serviceHost = new WorkflowServiceHost(
                new MainActivity(),
                new Uri(baseAddress));
            serviceHost.Description.Behaviors.Add(new ServiceMetadataBehavior()
            {
                HttpGetEnabled = true,
            });
            serviceHost.Open();

            Console.WriteLine("Host started, address: " + baseAddress);
            Console.ReadLine();
        }
    }
}
