namespace Articles.Workflows.ActivityAndThreading
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
            this.m_cmdWFInvoker = new System.Windows.Forms.Button();
            this.m_cmdWFInvokerAsync = new System.Windows.Forms.Button();
            this.m_cmdWFApplication = new System.Windows.Forms.Button();
            this.m_cmdObjectThread = new System.Windows.Forms.Button();
            this.m_barDummyProcess = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.m_lstItems = new System.Windows.Forms.ListBox();
            this.m_lblActivity = new System.Windows.Forms.Label();
            this.m_cboActivity = new System.Windows.Forms.ComboBox();
            this.m_cmdClose = new System.Windows.Forms.Button();
            this.m_cmdClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_cmdWFInvoker
            // 
            this.m_cmdWFInvoker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cmdWFInvoker.Location = new System.Drawing.Point(15, 39);
            this.m_cmdWFInvoker.Name = "m_cmdWFInvoker";
            this.m_cmdWFInvoker.Size = new System.Drawing.Size(639, 23);
            this.m_cmdWFInvoker.TabIndex = 0;
            this.m_cmdWFInvoker.Text = "WorkflowInvoker.Invoke";
            this.m_cmdWFInvoker.UseVisualStyleBackColor = true;
            this.m_cmdWFInvoker.Click += new System.EventHandler(this.OnCmdWFInvokerClick);
            // 
            // m_cmdWFInvokerAsync
            // 
            this.m_cmdWFInvokerAsync.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cmdWFInvokerAsync.Location = new System.Drawing.Point(15, 68);
            this.m_cmdWFInvokerAsync.Name = "m_cmdWFInvokerAsync";
            this.m_cmdWFInvokerAsync.Size = new System.Drawing.Size(639, 23);
            this.m_cmdWFInvokerAsync.TabIndex = 1;
            this.m_cmdWFInvokerAsync.Text = "WorkflowInvoker.InvokeAsync";
            this.m_cmdWFInvokerAsync.UseVisualStyleBackColor = true;
            this.m_cmdWFInvokerAsync.Click += new System.EventHandler(this.OnCmdWFInvokerAsyncClick);
            // 
            // m_cmdWFApplication
            // 
            this.m_cmdWFApplication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cmdWFApplication.Location = new System.Drawing.Point(15, 97);
            this.m_cmdWFApplication.Name = "m_cmdWFApplication";
            this.m_cmdWFApplication.Size = new System.Drawing.Size(639, 23);
            this.m_cmdWFApplication.TabIndex = 2;
            this.m_cmdWFApplication.Text = "WorkflowApplication.Run";
            this.m_cmdWFApplication.UseVisualStyleBackColor = true;
            this.m_cmdWFApplication.Click += new System.EventHandler(this.OnCmdWFApplicationRunClick);
            // 
            // m_cmdObjectThread
            // 
            this.m_cmdObjectThread.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cmdObjectThread.Location = new System.Drawing.Point(15, 126);
            this.m_cmdObjectThread.Name = "m_cmdObjectThread";
            this.m_cmdObjectThread.Size = new System.Drawing.Size(639, 23);
            this.m_cmdObjectThread.TabIndex = 3;
            this.m_cmdObjectThread.Text = "ObjectThread";
            this.m_cmdObjectThread.UseVisualStyleBackColor = true;
            this.m_cmdObjectThread.Click += new System.EventHandler(this.OnCmdObjectThreadClick);
            // 
            // m_barDummyProcess
            // 
            this.m_barDummyProcess.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_barDummyProcess.Location = new System.Drawing.Point(15, 467);
            this.m_barDummyProcess.MarqueeAnimationSpeed = 10;
            this.m_barDummyProcess.Name = "m_barDummyProcess";
            this.m_barDummyProcess.Size = new System.Drawing.Size(639, 23);
            this.m_barDummyProcess.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.m_barDummyProcess.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 171);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Items";
            // 
            // m_lstItems
            // 
            this.m_lstItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_lstItems.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lstItems.FormattingEnabled = true;
            this.m_lstItems.IntegralHeight = false;
            this.m_lstItems.ItemHeight = 23;
            this.m_lstItems.Location = new System.Drawing.Point(15, 187);
            this.m_lstItems.Name = "m_lstItems";
            this.m_lstItems.Size = new System.Drawing.Size(639, 274);
            this.m_lstItems.TabIndex = 7;
            // 
            // m_lblActivity
            // 
            this.m_lblActivity.AutoSize = true;
            this.m_lblActivity.Location = new System.Drawing.Point(12, 15);
            this.m_lblActivity.Name = "m_lblActivity";
            this.m_lblActivity.Size = new System.Drawing.Size(44, 13);
            this.m_lblActivity.TabIndex = 8;
            this.m_lblActivity.Text = "Activity:";
            // 
            // m_cboActivity
            // 
            this.m_cboActivity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cboActivity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboActivity.FormattingEnabled = true;
            this.m_cboActivity.Location = new System.Drawing.Point(127, 12);
            this.m_cboActivity.Name = "m_cboActivity";
            this.m_cboActivity.Size = new System.Drawing.Size(527, 21);
            this.m_cboActivity.TabIndex = 9;
            // 
            // m_cmdClose
            // 
            this.m_cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cmdClose.Location = new System.Drawing.Point(552, 496);
            this.m_cmdClose.Name = "m_cmdClose";
            this.m_cmdClose.Size = new System.Drawing.Size(102, 23);
            this.m_cmdClose.TabIndex = 10;
            this.m_cmdClose.Text = "Close";
            this.m_cmdClose.UseVisualStyleBackColor = true;
            this.m_cmdClose.Click += new System.EventHandler(this.OnCmdCloseClick);
            // 
            // m_cmdClear
            // 
            this.m_cmdClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cmdClear.Location = new System.Drawing.Point(552, 166);
            this.m_cmdClear.Name = "m_cmdClear";
            this.m_cmdClear.Size = new System.Drawing.Size(102, 23);
            this.m_cmdClear.TabIndex = 11;
            this.m_cmdClear.Text = "Clear";
            this.m_cmdClear.UseVisualStyleBackColor = true;
            this.m_cmdClear.Click += new System.EventHandler(this.OnCmdClearClick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 531);
            this.Controls.Add(this.m_cmdClear);
            this.Controls.Add(this.m_cmdClose);
            this.Controls.Add(this.m_cboActivity);
            this.Controls.Add(this.m_lblActivity);
            this.Controls.Add(this.m_lstItems);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_barDummyProcess);
            this.Controls.Add(this.m_cmdObjectThread);
            this.Controls.Add(this.m_cmdWFApplication);
            this.Controls.Add(this.m_cmdWFInvokerAsync);
            this.Controls.Add(this.m_cmdWFInvoker);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(450, 400);
            this.Name = "MainWindow";
            this.Text = "WF4 Samples - Activity and Threading";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_cmdWFInvoker;
        private System.Windows.Forms.Button m_cmdWFInvokerAsync;
        private System.Windows.Forms.Button m_cmdWFApplication;
        private System.Windows.Forms.Button m_cmdObjectThread;
        private System.Windows.Forms.ProgressBar m_barDummyProcess;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox m_lstItems;
        private System.Windows.Forms.Label m_lblActivity;
        private System.Windows.Forms.ComboBox m_cboActivity;
        private System.Windows.Forms.Button m_cmdClose;
        private System.Windows.Forms.Button m_cmdClear;
    }
}

