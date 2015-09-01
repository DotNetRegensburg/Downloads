using System.Activities;

namespace Articles.Workflow.StateMachineWorkflow
{

    class Program
    {
        static void Main(string[] args)
        {
            WorkflowInvoker.Invoke(new MainActivity());
        }
    }
}
