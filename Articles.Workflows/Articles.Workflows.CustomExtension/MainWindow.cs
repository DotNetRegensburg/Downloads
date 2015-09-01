using System;
using System.Activities;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Activities.Tracking;

namespace Articles.Workflows.CustomExtension
{
    public partial class MainWindow : Form
    {
        private MainActivity m_mainActivity;
        private WorkflowInvoker m_mainActivityInvoker;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Adds an item to the ListBox.
        /// </summary>
        /// <param name="itemText">The text to add.</param>
        public void AddItem(string itemText)
        {
            m_lstItems.Items.Add(itemText);
            m_lstItems.SelectedIndex = m_lstItems.Items.Count - 1;
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
        /// Called when user wants to execute the workflow.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnCmdCallWorkflowClick(object sender, EventArgs e)
        {
            //Prepare activity
            if (m_mainActivity == null) { m_mainActivity = new MainActivity(); }
            if (m_mainActivityInvoker == null) 
            {
                //Create invoker
                m_mainActivityInvoker = new WorkflowInvoker(m_mainActivity);
                m_mainActivityInvoker.Extensions.Add<IMainWindow>(() => new MainWindowAccessor(this));

                //Apply tracking
                TrackingProfile trackingProfile = new TrackingProfile();
                DebugTrackingParticipant trackingParticipant = new DebugTrackingParticipant();
                m_mainActivityInvoker.Extensions.Add(trackingParticipant);
            }

            //Call workflow
            Dictionary<string, object> arguments = new Dictionary<string, object>();
            arguments.Add("GivenValue", m_txtInput.Text);
            m_mainActivityInvoker.InvokeAsync(arguments);
        }

        /// <summary>
        /// Called when user wants to clear all list items.
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
