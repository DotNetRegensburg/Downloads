using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Activities;
using System.ServiceModel.Activities;
using System.ServiceModel.Description;

namespace Articles.Workflow.CustomWorkflowService
{
    public partial class ServerHostWindow : Form
    {
        private WorkflowServiceHost m_serviceHost;
        private Activity m_activity;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerHostWindow"/> class.
        /// </summary>
        public ServerHostWindow(WorkflowServiceHost serviceHost, Activity workflow)
        {
            InitializeComponent();

            m_serviceHost = serviceHost;
            m_activity = workflow;
        }

        /// <summary>
        /// Standard load event.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            m_txtUrl.Text = m_serviceHost.BaseAddresses[0].ToString();
            
            m_webBrowserService.Navigate(m_serviceHost.BaseAddresses[0].ToString());
            m_webBrowserWsdl.Navigate(m_serviceHost.BaseAddresses[0].ToString() + "?wsdl");
        }

        /// <summary>
        /// Called when user wants to close this window.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnCmdCloseClick(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Called when user wants to follow the service link.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
        private void OnTxtUrlClick(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(m_txtUrl.Text);
        }
    }
}
