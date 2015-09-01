namespace Articles.WorkflowScripting
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.m_menuBar = new System.Windows.Forms.MenuStrip();
            this.m_mnuScene = new System.Windows.Forms.ToolStripMenuItem();
            this.m_mnuScenePlain = new System.Windows.Forms.ToolStripMenuItem();
            this.m_mnuSceneMotionBlur = new System.Windows.Forms.ToolStripMenuItem();
            this.m_mnuWorkflows = new System.Windows.Forms.ToolStripMenuItem();
            this.m_view3D = new Common.GraphicsEngine.Gui.Direct3D11SceneView();
            this.m_splitter = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_lstRunningWorkflow = new System.Windows.Forms.ListBox();
            this.m_bindingSourceWorkflows = new System.Windows.Forms.BindingSource(this.components);
            this.m_bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.m_lblRunningWokflow = new System.Windows.Forms.Label();
            this.m_refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.m_menuBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_splitter)).BeginInit();
            this.m_splitter.Panel1.SuspendLayout();
            this.m_splitter.Panel2.SuspendLayout();
            this.m_splitter.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_bindingSourceWorkflows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // m_menuBar
            // 
            this.m_menuBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.m_menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_mnuScene,
            this.m_mnuWorkflows});
            this.m_menuBar.Location = new System.Drawing.Point(0, 0);
            this.m_menuBar.Name = "m_menuBar";
            this.m_menuBar.Size = new System.Drawing.Size(725, 24);
            this.m_menuBar.TabIndex = 1;
            // 
            // m_mnuScene
            // 
            this.m_mnuScene.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_mnuScenePlain,
            this.m_mnuSceneMotionBlur});
            this.m_mnuScene.Name = "m_mnuScene";
            this.m_mnuScene.Size = new System.Drawing.Size(50, 20);
            this.m_mnuScene.Text = "Scene";
            // 
            // m_mnuScenePlain
            // 
            this.m_mnuScenePlain.Name = "m_mnuScenePlain";
            this.m_mnuScenePlain.Size = new System.Drawing.Size(137, 22);
            this.m_mnuScenePlain.Text = "Plain";
            this.m_mnuScenePlain.Click += new System.EventHandler(this.OnMnuScenePlainClick);
            // 
            // m_mnuSceneMotionBlur
            // 
            this.m_mnuSceneMotionBlur.Name = "m_mnuSceneMotionBlur";
            this.m_mnuSceneMotionBlur.Size = new System.Drawing.Size(137, 22);
            this.m_mnuSceneMotionBlur.Text = "Motion Blur";
            this.m_mnuSceneMotionBlur.Click += new System.EventHandler(this.OnMnuSceneMotionBlurClick);
            // 
            // m_mnuWorkflows
            // 
            this.m_mnuWorkflows.Name = "m_mnuWorkflows";
            this.m_mnuWorkflows.Size = new System.Drawing.Size(75, 20);
            this.m_mnuWorkflows.Text = "Workflows";
            // 
            // m_view3D
            // 
            this.m_view3D.BackColor = System.Drawing.Color.White;
            this.m_view3D.CyclicRendering = true;
            this.m_view3D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_view3D.IsMovementEnabled = true;
            this.m_view3D.IsWireframeEnabled = false;
            this.m_view3D.Location = new System.Drawing.Point(0, 0);
            this.m_view3D.Name = "m_view3D";
            this.m_view3D.Size = new System.Drawing.Size(528, 415);
            this.m_view3D.TabIndex = 0;
            this.m_view3D.Text = "direct3D11SceneView1";
            // 
            // m_splitter
            // 
            this.m_splitter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.m_splitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_splitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.m_splitter.Location = new System.Drawing.Point(0, 24);
            this.m_splitter.Name = "m_splitter";
            // 
            // m_splitter.Panel1
            // 
            this.m_splitter.Panel1.BackColor = System.Drawing.Color.White;
            this.m_splitter.Panel1.Controls.Add(this.panel1);
            // 
            // m_splitter.Panel2
            // 
            this.m_splitter.Panel2.Controls.Add(this.m_view3D);
            this.m_splitter.Size = new System.Drawing.Size(725, 415);
            this.m_splitter.SplitterDistance = 193;
            this.m_splitter.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.m_lstRunningWorkflow);
            this.panel1.Controls.Add(this.m_lblRunningWokflow);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(193, 415);
            this.panel1.TabIndex = 2;
            // 
            // m_lstRunningWorkflow
            // 
            this.m_lstRunningWorkflow.DataSource = this.m_bindingSourceWorkflows;
            this.m_lstRunningWorkflow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lstRunningWorkflow.FormattingEnabled = true;
            this.m_lstRunningWorkflow.IntegralHeight = false;
            this.m_lstRunningWorkflow.Location = new System.Drawing.Point(0, 20);
            this.m_lstRunningWorkflow.Name = "m_lstRunningWorkflow";
            this.m_lstRunningWorkflow.Size = new System.Drawing.Size(191, 393);
            this.m_lstRunningWorkflow.TabIndex = 1;
            // 
            // m_bindingSourceWorkflows
            // 
            this.m_bindingSourceWorkflows.DataMember = "RunningWorkflows";
            this.m_bindingSourceWorkflows.DataSource = this.m_bindingSource;
            // 
            // m_bindingSource
            // 
            this.m_bindingSource.DataSource = typeof(Articles.WorkflowScripting.Gui.MainWindowDataSource);
            // 
            // m_lblRunningWokflow
            // 
            this.m_lblRunningWokflow.AutoSize = true;
            this.m_lblRunningWokflow.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_lblRunningWokflow.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblRunningWokflow.Location = new System.Drawing.Point(0, 0);
            this.m_lblRunningWokflow.Name = "m_lblRunningWokflow";
            this.m_lblRunningWokflow.Size = new System.Drawing.Size(146, 20);
            this.m_lblRunningWokflow.TabIndex = 1;
            this.m_lblRunningWokflow.Text = "Running Workflows";
            // 
            // m_refreshTimer
            // 
            this.m_refreshTimer.Enabled = true;
            this.m_refreshTimer.Interval = 300;
            this.m_refreshTimer.Tick += new System.EventHandler(this.OnRefreshTimerTick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 439);
            this.Controls.Add(this.m_splitter);
            this.Controls.Add(this.m_menuBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.m_menuBar;
            this.Name = "MainWindow";
            this.Text = "WF4 Samples - Workflow Scripting";
            this.m_menuBar.ResumeLayout(false);
            this.m_menuBar.PerformLayout();
            this.m_splitter.Panel1.ResumeLayout(false);
            this.m_splitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_splitter)).EndInit();
            this.m_splitter.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_bindingSourceWorkflows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_bindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Common.GraphicsEngine.Gui.Direct3D11SceneView m_view3D;
        private System.Windows.Forms.MenuStrip m_menuBar;
        private System.Windows.Forms.ToolStripMenuItem m_mnuWorkflows;
        private System.Windows.Forms.SplitContainer m_splitter;
        private System.Windows.Forms.BindingSource m_bindingSource;
        private System.Windows.Forms.ToolStripMenuItem m_mnuScene;
        private System.Windows.Forms.ToolStripMenuItem m_mnuScenePlain;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem m_mnuSceneMotionBlur;
        private System.Windows.Forms.Timer m_refreshTimer;
        private System.Windows.Forms.ListBox m_lstRunningWorkflow;
        private System.Windows.Forms.BindingSource m_bindingSourceWorkflows;
        private System.Windows.Forms.Label m_lblRunningWokflow;
    }
}

