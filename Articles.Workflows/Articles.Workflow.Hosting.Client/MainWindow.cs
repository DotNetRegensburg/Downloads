using System;
using System.Windows.Forms;

namespace Articles.Workflow.Hosting.Client
{
    public partial class MainWindow : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when user clicks close.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnCmdCloseClick(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Called when user wants to send a message.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnCmdCallPrintClick(object sender, EventArgs e)
        {
            WFService.ServiceMainActivityClient client = new WFService.ServiceMainActivityClient();
            client.Print(m_txtMessageSent.Text);
            m_txtMessageReceived.Text = string.Empty;
        }

        /// <summary>
        /// Called when user wants to send a message.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnCmdCallPrintExtendedClick(object sender, EventArgs e)
        {
            WFService.ServiceMainActivityClient client = new WFService.ServiceMainActivityClient();
            client.PrintExtended(m_txtMessageSent.Text);
            m_txtMessageReceived.Text = string.Empty;
        }

        /// <summary>
        /// Called when user wants to get a message.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnCmdCallReadClick(object sender, EventArgs e)
        {
            WFService.ServiceMainActivityClient client = new WFService.ServiceMainActivityClient();
            m_txtMessageReceived.Text = client.Read(m_txtMessageSent.Text);
        }
    }
}
