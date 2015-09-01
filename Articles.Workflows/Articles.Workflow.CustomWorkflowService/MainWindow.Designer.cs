namespace Articles.Workflow.CustomWorkflowService
{
    partial class MainWindow
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.m_mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.m_mnuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.m_mnuSplitter = new System.Windows.Forms.ToolStripSeparator();
            this.m_mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.m_mnuCompileAndRun = new System.Windows.Forms.ToolStripMenuItem();
            this.m_statusBar = new System.Windows.Forms.StatusStrip();
            this.m_wpfHost = new System.Windows.Forms.Integration.ElementHost();
            this.m_dlgSaveFile = new System.Windows.Forms.SaveFileDialog();
            this.m_dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.m_panMainContent = new System.Windows.Forms.Panel();
            this.m_mainMenu.SuspendLayout();
            this.m_panMainContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_mainMenu
            // 
            this.m_mainMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.m_mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.m_mnuCompileAndRun});
            this.m_mainMenu.Location = new System.Drawing.Point(0, 0);
            this.m_mainMenu.Name = "m_mainMenu";
            this.m_mainMenu.Size = new System.Drawing.Size(740, 24);
            this.m_mainMenu.TabIndex = 0;
            this.m_mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_mnuOpen,
            this.m_mnuSave,
            this.m_mnuSplitter,
            this.m_mnuExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // m_mnuOpen
            // 
            this.m_mnuOpen.Name = "m_mnuOpen";
            this.m_mnuOpen.Size = new System.Drawing.Size(103, 22);
            this.m_mnuOpen.Text = "Open";
            this.m_mnuOpen.Click += new System.EventHandler(this.OnMnuOpenClick);
            // 
            // m_mnuSave
            // 
            this.m_mnuSave.Name = "m_mnuSave";
            this.m_mnuSave.Size = new System.Drawing.Size(103, 22);
            this.m_mnuSave.Text = "Save";
            this.m_mnuSave.Click += new System.EventHandler(this.OnMnuSaveClick);
            // 
            // m_mnuSplitter
            // 
            this.m_mnuSplitter.Name = "m_mnuSplitter";
            this.m_mnuSplitter.Size = new System.Drawing.Size(100, 6);
            // 
            // m_mnuExit
            // 
            this.m_mnuExit.Name = "m_mnuExit";
            this.m_mnuExit.Size = new System.Drawing.Size(103, 22);
            this.m_mnuExit.Text = "Exit";
            this.m_mnuExit.Click += new System.EventHandler(this.OnMnuExitClick);
            // 
            // m_mnuCompileAndRun
            // 
            this.m_mnuCompileAndRun.Image = global::Articles.Workflow.CustomWorkflowService.Properties.Resources.IconCompileAndRun16x16;
            this.m_mnuCompileAndRun.Name = "m_mnuCompileAndRun";
            this.m_mnuCompileAndRun.Size = new System.Drawing.Size(130, 20);
            this.m_mnuCompileAndRun.Text = "Compile and Run!";
            this.m_mnuCompileAndRun.Click += new System.EventHandler(this.OnMnuCompileAndRunClick);
            // 
            // m_statusBar
            // 
            this.m_statusBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.m_statusBar.Location = new System.Drawing.Point(0, 362);
            this.m_statusBar.Name = "m_statusBar";
            this.m_statusBar.Size = new System.Drawing.Size(740, 22);
            this.m_statusBar.TabIndex = 1;
            this.m_statusBar.Text = "statusStrip1";
            // 
            // m_wpfHost
            // 
            this.m_wpfHost.BackColor = System.Drawing.Color.White;
            this.m_wpfHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_wpfHost.Location = new System.Drawing.Point(5, 5);
            this.m_wpfHost.Name = "m_wpfHost";
            this.m_wpfHost.Size = new System.Drawing.Size(730, 328);
            this.m_wpfHost.TabIndex = 2;
            this.m_wpfHost.Child = null;
            // 
            // m_dlgSaveFile
            // 
            this.m_dlgSaveFile.DefaultExt = "xml";
            this.m_dlgSaveFile.Filter = "Xml-Files (*.xml)|*.xml|Xaml-Files (*.xaml)|*.xaml|All Files|*.*";
            this.m_dlgSaveFile.RestoreDirectory = true;
            // 
            // m_dlgOpenFile
            // 
            this.m_dlgOpenFile.DefaultExt = "xml";
            this.m_dlgOpenFile.Filter = "Xml-Files (*.xml)|*.xml|Xaml-Files (*.xaml)|*.xaml|All Files|*.*";
            this.m_dlgOpenFile.RestoreDirectory = true;
            // 
            // m_panMainContent
            // 
            this.m_panMainContent.BackColor = System.Drawing.Color.AliceBlue;
            this.m_panMainContent.Controls.Add(this.m_wpfHost);
            this.m_panMainContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_panMainContent.Location = new System.Drawing.Point(0, 24);
            this.m_panMainContent.Name = "m_panMainContent";
            this.m_panMainContent.Padding = new System.Windows.Forms.Padding(5);
            this.m_panMainContent.Size = new System.Drawing.Size(740, 338);
            this.m_panMainContent.TabIndex = 3;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 384);
            this.Controls.Add(this.m_panMainContent);
            this.Controls.Add(this.m_statusBar);
            this.Controls.Add(this.m_mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.m_mainMenu;
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "MainWindow";
            this.Text = "Custom Workflow Server - Editor";
            this.m_mainMenu.ResumeLayout(false);
            this.m_mainMenu.PerformLayout();
            this.m_panMainContent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip m_mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.StatusStrip m_statusBar;
        private System.Windows.Forms.Integration.ElementHost m_wpfHost;
        private System.Windows.Forms.ToolStripMenuItem m_mnuSave;
        private System.Windows.Forms.SaveFileDialog m_dlgSaveFile;
        private System.Windows.Forms.ToolStripMenuItem m_mnuOpen;
        private System.Windows.Forms.ToolStripMenuItem m_mnuExit;
        private System.Windows.Forms.OpenFileDialog m_dlgOpenFile;
        private System.Windows.Forms.ToolStripSeparator m_mnuSplitter;
        private System.Windows.Forms.ToolStripMenuItem m_mnuCompileAndRun;
        private System.Windows.Forms.Panel m_panMainContent;
    }
}

