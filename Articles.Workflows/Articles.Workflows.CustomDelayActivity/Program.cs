using System.Activities;

namespace Articles.Workflows.CustomDelayActivity
{
    public static class Program
    {
        /// <summary>
        /// Main method.
        /// </summary>
        public static void Main()
        {
            WorkflowApplication wfApplication = new WorkflowApplication(new MainActivity());
            wfApplication.Run();
        }
    }
}
