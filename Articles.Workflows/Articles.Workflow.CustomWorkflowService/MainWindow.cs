using System;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Activities;
using System.Activities.Statements;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Markup;
using System.ServiceModel.Activities;
using System.ServiceModel.Description;

namespace Articles.Workflow.CustomWorkflowService
{
    public partial class MainWindow : Form
    {
        private ServiceEditor m_serviceEditor;

        private string m_serverFilePath;
        private Process m_serverProcess;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Update current state of the dialog.
        /// </summary>
        private void UpdateDialogState()
        {
            //m_panMainContent.Enabled = m_serverProcess == null;
            if (m_wpfHost.Child != null) { m_wpfHost.Child.IsEnabled = m_serverProcess == null; }
            m_mainMenu.Enabled = m_serverProcess == null;
            m_statusBar.Enabled = m_statusBar == null;
        }

        /// <summary>
        /// Standard load event.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!base.DesignMode)
            {
                m_serviceEditor = new ServiceEditor();
                m_wpfHost.Child = m_serviceEditor;
                m_wpfHost.PropertyMap.ApplyAll();
            }

            UpdateDialogState();
        }

        /// <summary>
        /// Called when user wants to save the service.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnMnuSaveClick(object sender, EventArgs e)
        {
            if (m_dlgSaveFile.ShowDialog(this) == DialogResult.OK)
            {
                m_serviceEditor.PerformSave(m_dlgSaveFile.FileName);
            }
        }

        /// <summary>
        /// Called when user wants to open a service.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnMnuOpenClick(object sender, EventArgs e)
        {
            if (m_dlgOpenFile.ShowDialog(this) == DialogResult.OK)
            {
                m_serviceEditor.PerformLoad(m_dlgOpenFile.FileName);
            }
        }

        /// <summary>
        /// Called when user wants to close the application.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnMnuExitClick(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Called when the form has closed.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (m_serverProcess != null)
            {
                try { m_serverProcess.Kill(); }
                catch { }
                m_serverProcess = null;
            }
        }

        /// <summary>
        /// Called when user wants to run the server.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnMnuCompileAndRunClick(object sender, EventArgs e)
        {
            m_serverFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                Guid.NewGuid().ToString() + ".xml");
            m_serviceEditor.PerformSave(m_serverFilePath);

            try
            {
                m_serverProcess = Process.Start(new ProcessStartInfo()
                {
                    FileName = Assembly.GetExecutingAssembly().Location,
                    Arguments = String.Format("-runserver \"{0}\" \"{1}\"", m_serverFilePath, this.Handle),
                    Verb = "runas",
                });
                m_serverProcess.EnableRaisingEvents = true;
                m_serverProcess.Exited += new EventHandler(OnServerProcessExited);
            }
            catch (Exception)
            {
                if (File.Exists(m_serverFilePath))
                {
                    try { File.Delete(m_serverFilePath); }
                    catch { }
                }
            }

            UpdateDialogState();
        }

        /// <summary>
        /// Called when server process has exited.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnServerProcessExited(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(() =>
            {
                m_serverProcess = null;
                if (File.Exists(m_serverFilePath))
                {
                    try { File.Delete(m_serverFilePath); }
                    catch { }
                }

                UpdateDialogState();
            }));
        }
    }
}
