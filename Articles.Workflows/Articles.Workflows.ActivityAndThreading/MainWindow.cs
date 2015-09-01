using System;
using System.Activities;
using System.Threading;
using System.Windows.Forms;
using Articles.Workflows.ActivityAndThreading.Util;

namespace Articles.Workflows.ActivityAndThreading
{
    public partial class MainWindow : Form
    {
        private ObjectThread m_objectThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            //Create activities
            m_cboActivity.Items.Add(new MainActivity());
            m_cboActivity.Items.Add(new MainActivityParallel());
            m_cboActivity.Items.Add(new MainActivityBlocking());
            m_cboActivity.Items.Add(new MainActivityBlockingParallel());
            m_cboActivity.SelectedIndex = 0;
        }

        /// <summary>
        /// Adds an item to the ListBox.
        /// </summary>
        /// <param name="itemText">The text to add.</param>
        public void AddItem(string itemText)
        {
            if (base.InvokeRequired)
            {
                int threadID = Thread.CurrentThread.ManagedThreadId;
                this.BeginInvoke(new Action(() =>
                {
                    m_lstItems.Items.Add("[Thread " + threadID + "] " + itemText);
                    m_lstItems.SelectedIndex = m_lstItems.Items.Count - 1;
                }));
            }
            else
            {
                m_lstItems.Items.Add("[GUI-Thread] " + itemText);
                m_lstItems.SelectedIndex = m_lstItems.Items.Count - 1;
            }
        }

        /// <summary>
        /// Called when user wants to use WorkflowInvoker.Invoke.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnCmdWFInvokerClick(object sender, EventArgs e)
        {
            WorkflowInvoker invoker = new WorkflowInvoker(m_cboActivity.SelectedItem as Activity);
            invoker.Extensions.Add<IMainWindow>(() => new MainWindowAccessor(this));
            invoker.Invoke();
        }

        /// <summary>
        /// Called when user wants to use WorkflowInvoker.InvokeAsync.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnCmdWFInvokerAsyncClick(object sender, EventArgs e)
        {
            WorkflowInvoker invoker = new WorkflowInvoker(m_cboActivity.SelectedItem as Activity);
            invoker.Extensions.Add<IMainWindow>(() => new MainWindowAccessor(this));
            invoker.InvokeAsync();
        }

        /// <summary>
        /// Called when user wants to use WorkflowApplication.Run.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnCmdWFApplicationRunClick(object sender, EventArgs e)
        {
            WorkflowApplication wfApplication = new WorkflowApplication(m_cboActivity.SelectedItem as Activity);
            wfApplication.Extensions.Add<IMainWindow>(() => new MainWindowAccessor(this));
            wfApplication.Run();
        }

        /// <summary>
        /// Called when user wants to use ObjectThread.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnCmdObjectThreadClick(object sender, EventArgs e)
        {
            //Initialize ObjectThread if not done before
            if (m_objectThread == null)
            {
                m_objectThread = new ObjectThread();
                m_objectThread.Start();
            }

            //Execute the activity
            m_objectThread.BeginInvoke(
                m_cboActivity.SelectedItem as Activity,
                (wfApp) => wfApp.Extensions.Add<IMainWindow>(() => new MainWindowAccessor(this)));
        }

        /// <summary>
        /// Called when user wants to close.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnCmdCloseClick(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Called when user wants to clear the item list.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnCmdClearClick(object sender, EventArgs e)
        {
            m_lstItems.Items.Clear();
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        private class MainWindowAccessor : IMainWindow
        {
            public MainWindow MainWindow { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="MainWindowAccessor"/> class.
            /// </summary>
            /// <param name="owner">The owner.</param>
            public MainWindowAccessor(MainWindow owner)
            {
                this.MainWindow = owner;
            }

            /// <summary>
            /// Adds an item to the ListBox.
            /// </summary>
            /// <param name="itemText">The text to add.</param>
            public void AddItem(string itemText)
            {
                MainWindow.AddItem(itemText);
            }
        }
    }
}
