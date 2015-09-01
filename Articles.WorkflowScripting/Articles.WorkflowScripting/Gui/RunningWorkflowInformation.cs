using System.Activities;
using System.Drawing;

namespace Articles.WorkflowScripting.Gui
{
    public class RunningWorkflowInformation 
    {
        private string m_workflowName;
        private WorkflowApplication m_workflowApplication;

        /// <summary>
        /// Initializes a new instance of the <see cref="RunningWorkflowInformation"/> class.
        /// </summary>
        /// <param name="name">Display name of the workflow.</param>
        /// <param name="wfApplication">The application object hosting the workflow.</param>
        public RunningWorkflowInformation(string name, WorkflowApplication wfApplication)
        {
            m_workflowName = name;
            m_workflowApplication = wfApplication;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return m_workflowName;
        }

        /// <summary>
        /// Gets or sets the name of the workflow.
        /// </summary>
        public string WorkflowName
        {
            get { return m_workflowName; }
        }

        /// <summary>
        /// Gets the state image.
        /// </summary>
        public Image StateImage
        {
            get { return Properties.Resources.IconRefresh16x16; }
        }
    }
}
