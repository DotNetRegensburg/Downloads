namespace Articles.Workflows.CustomExtension
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
            this.m_cmdClose = new System.Windows.Forms.Button();
            this.m_lstItems = new System.Windows.Forms.ListBox();
            this.m_lblInput = new System.Windows.Forms.Label();
            this.m_txtInput = new System.Windows.Forms.TextBox();
            this.m_cmdCallWorkflow = new System.Windows.Forms.Button();
            this.m_cmdClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_cmdClose
            // 
            this.m_cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cmdClose.Location = new System.Drawing.Point(547, 496);
            this.m_cmdClose.Name = "m_cmdClose";
            this.m_cmdClose.Size = new System.Drawing.Size(110, 23);
            this.m_cmdClose.TabIndex = 0;
            this.m_cmdClose.Text = "Close";
            this.m_cmdClose.UseVisualStyleBackColor = true;
            this.m_cmdClose.Click += new System.EventHandler(this.OnCmdCloseClick);
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
            this.m_lstItems.Location = new System.Drawing.Point(12, 38);
            this.m_lstItems.Name = "m_lstItems";
            this.m_lstItems.Size = new System.Drawing.Size(645, 452);
            this.m_lstItems.TabIndex = 1;
            // 
            // m_lblInput
            // 
            this.m_lblInput.AutoSize = true;
            this.m_lblInput.Location = new System.Drawing.Point(9, 15);
            this.m_lblInput.Name = "m_lblInput";
            this.m_lblInput.Size = new System.Drawing.Size(102, 13);
            this.m_lblInput.TabIndex = 2;
            this.m_lblInput.Text = "Workflow argument:";
            // 
            // m_txtInput
            // 
            this.m_txtInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_txtInput.Location = new System.Drawing.Point(133, 12);
            this.m_txtInput.Name = "m_txtInput";
            this.m_txtInput.Size = new System.Drawing.Size(245, 20);
            this.m_txtInput.TabIndex = 3;
            // 
            // m_cmdCallWorkflow
            // 
            this.m_cmdCallWorkflow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cmdCallWorkflow.Image = global::Articles.Workflows.CustomExtension.Properties.Resources.IconCogGo16x16;
            this.m_cmdCallWorkflow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_cmdCallWorkflow.Location = new System.Drawing.Point(384, 10);
            this.m_cmdCallWorkflow.Name = "m_cmdCallWorkflow";
            this.m_cmdCallWorkflow.Size = new System.Drawing.Size(157, 23);
            this.m_cmdCallWorkflow.TabIndex = 4;
            this.m_cmdCallWorkflow.Text = "Call Workflow!";
            this.m_cmdCallWorkflow.UseVisualStyleBackColor = true;
            this.m_cmdCallWorkflow.Click += new System.EventHandler(this.OnCmdCallWorkflowClick);
            // 
            // m_cmdClear
            // 
            this.m_cmdClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cmdClear.Location = new System.Drawing.Point(547, 10);
            this.m_cmdClear.Name = "m_cmdClear";
            this.m_cmdClear.Size = new System.Drawing.Size(110, 23);
            this.m_cmdClear.TabIndex = 5;
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
            this.Controls.Add(this.m_cmdCallWorkflow);
            this.Controls.Add(this.m_txtInput);
            this.Controls.Add(this.m_lblInput);
            this.Controls.Add(this.m_lstItems);
            this.Controls.Add(this.m_cmdClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "WF4 Samples - Custom Extension";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_cmdClose;
        private System.Windows.Forms.ListBox m_lstItems;
        private System.Windows.Forms.Label m_lblInput;
        private System.Windows.Forms.TextBox m_txtInput;
        private System.Windows.Forms.Button m_cmdCallWorkflow;
        private System.Windows.Forms.Button m_cmdClear;
    }
}

