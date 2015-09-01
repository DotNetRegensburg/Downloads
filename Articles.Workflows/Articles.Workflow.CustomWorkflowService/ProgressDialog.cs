using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Articles.Workflow.CustomWorkflowService
{
    public partial class ProgressDialog : Form
    {
        private Action m_backgroundAction;
        private Exception m_occurredException;

        /// <summary>
        /// Prevents a default instance of the <see cref="ProgressDialog"/> class from being created.
        /// </summary>
        private ProgressDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Shows the ProgressDialog and performs the given action in background.
        /// </summary>
        /// <param name="actionToPerform">The action to perform.</param>
        public static DialogResult Show(Action actionToPerform)
        {
            return Show(null, actionToPerform);
        }

        /// <summary>
        /// Shows the ProgressDialog and performs the given action in background.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="actionToPerform">The action to perform.</param>
        public static DialogResult Show(IWin32Window owner, Action actionToPerform)
        {
            if (actionToPerform == null) { throw new ArgumentNullException("actionToPerform"); }

            using (ProgressDialog progressDialog = new ProgressDialog())
            {
                progressDialog.m_backgroundAction = actionToPerform;
                if (owner == null) { return progressDialog.ShowDialog(); }
                else { return progressDialog.ShowDialog(owner); }
            }
        }

        /// <summary>
        /// Standard load event.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Starts background work.
            StartBackgroundWork();
        }

        /// <summary>
        /// Starts the background work.
        /// </summary>
        private void StartBackgroundWork()
        {
            BackgroundWorker reportWorker = new BackgroundWorker();
            reportWorker.DoWork += new DoWorkEventHandler(OnBackgroundWorkerDoWork);
            reportWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnBackgroundWorkerWorkCompleted);
            reportWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Called when BackgroundWorker has finished.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void OnBackgroundWorkerWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Throw occurred exception
            if (m_occurredException != null)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();

                throw new ApplicationException(
                    "Error while performing background progress, see inner exception for more detail!",
                    m_occurredException);
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// Called when backgroundworker starts with the work.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void OnBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                m_backgroundAction();
            }
            catch (Exception ex)
            {
                m_occurredException = ex;
            }
        }
    }
}
