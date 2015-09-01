using System.Collections.Generic;

namespace Articles.WorkflowScripting.Gui
{
    public class MainWindowDataSource
    {
        private List<RunningWorkflowInformation> m_runningWorkflows;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowDataSource"/> class.
        /// </summary>
        public MainWindowDataSource()
        {
            m_runningWorkflows = new List<RunningWorkflowInformation>();
        }

        /// <summary>
        /// Gets a collection containing all running workflows.
        /// </summary>
        public List<RunningWorkflowInformation> RunningWorkflows
        {
            get { return m_runningWorkflows; }
        }
    }
}
