namespace Articles.Workflow.Hosting.Client
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
            this.m_cmdCallPrint = new System.Windows.Forms.Button();
            this.m_lblMessageSent = new System.Windows.Forms.Label();
            this.m_txtMessageSent = new System.Windows.Forms.TextBox();
            this.m_txtMessageReceived = new System.Windows.Forms.TextBox();
            this.m_lblMessageReceived = new System.Windows.Forms.Label();
            this.m_cmdCallPrintExtended = new System.Windows.Forms.Button();
            this.m_cmdCallRead = new System.Windows.Forms.Button();
            this.m_cmdClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_cmdCallPrint
            // 
            this.m_cmdCallPrint.Image = ((System.Drawing.Image)(resources.GetObject("m_cmdCallPrint.Image")));
            this.m_cmdCallPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_cmdCallPrint.Location = new System.Drawing.Point(146, 64);
            this.m_cmdCallPrint.Name = "m_cmdCallPrint";
            this.m_cmdCallPrint.Size = new System.Drawing.Size(152, 23);
            this.m_cmdCallPrint.TabIndex = 0;
            this.m_cmdCallPrint.Text = "Call Print";
            this.m_cmdCallPrint.UseVisualStyleBackColor = true;
            this.m_cmdCallPrint.Click += new System.EventHandler(this.OnCmdCallPrintClick);
            // 
            // m_lblMessageSent
            // 
            this.m_lblMessageSent.AutoSize = true;
            this.m_lblMessageSent.Location = new System.Drawing.Point(12, 15);
            this.m_lblMessageSent.Name = "m_lblMessageSent";
            this.m_lblMessageSent.Size = new System.Drawing.Size(91, 13);
            this.m_lblMessageSent.TabIndex = 1;
            this.m_lblMessageSent.Text = "Message to send:";
            // 
            // m_txtMessageSent
            // 
            this.m_txtMessageSent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_txtMessageSent.Location = new System.Drawing.Point(146, 12);
            this.m_txtMessageSent.Name = "m_txtMessageSent";
            this.m_txtMessageSent.Size = new System.Drawing.Size(340, 20);
            this.m_txtMessageSent.TabIndex = 2;
            // 
            // m_txtMessageReceived
            // 
            this.m_txtMessageReceived.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_txtMessageReceived.Location = new System.Drawing.Point(146, 38);
            this.m_txtMessageReceived.Name = "m_txtMessageReceived";
            this.m_txtMessageReceived.ReadOnly = true;
            this.m_txtMessageReceived.Size = new System.Drawing.Size(340, 20);
            this.m_txtMessageReceived.TabIndex = 3;
            // 
            // m_lblMessageReceived
            // 
            this.m_lblMessageReceived.AutoSize = true;
            this.m_lblMessageReceived.Location = new System.Drawing.Point(12, 41);
            this.m_lblMessageReceived.Name = "m_lblMessageReceived";
            this.m_lblMessageReceived.Size = new System.Drawing.Size(97, 13);
            this.m_lblMessageReceived.TabIndex = 4;
            this.m_lblMessageReceived.Text = "Message received:";
            // 
            // m_cmdCallPrintExtended
            // 
            this.m_cmdCallPrintExtended.Image = ((System.Drawing.Image)(resources.GetObject("m_cmdCallPrintExtended.Image")));
            this.m_cmdCallPrintExtended.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_cmdCallPrintExtended.Location = new System.Drawing.Point(146, 93);
            this.m_cmdCallPrintExtended.Name = "m_cmdCallPrintExtended";
            this.m_cmdCallPrintExtended.Size = new System.Drawing.Size(152, 23);
            this.m_cmdCallPrintExtended.TabIndex = 5;
            this.m_cmdCallPrintExtended.Text = "Call PrintExtended";
            this.m_cmdCallPrintExtended.UseVisualStyleBackColor = true;
            this.m_cmdCallPrintExtended.Click += new System.EventHandler(this.OnCmdCallPrintExtendedClick);
            // 
            // m_cmdCallRead
            // 
            this.m_cmdCallRead.Image = ((System.Drawing.Image)(resources.GetObject("m_cmdCallRead.Image")));
            this.m_cmdCallRead.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_cmdCallRead.Location = new System.Drawing.Point(146, 122);
            this.m_cmdCallRead.Name = "m_cmdCallRead";
            this.m_cmdCallRead.Size = new System.Drawing.Size(152, 23);
            this.m_cmdCallRead.TabIndex = 6;
            this.m_cmdCallRead.Text = "Call Read";
            this.m_cmdCallRead.UseVisualStyleBackColor = true;
            this.m_cmdCallRead.Click += new System.EventHandler(this.OnCmdCallReadClick);
            // 
            // m_cmdClose
            // 
            this.m_cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cmdClose.Location = new System.Drawing.Point(357, 187);
            this.m_cmdClose.Name = "m_cmdClose";
            this.m_cmdClose.Size = new System.Drawing.Size(129, 23);
            this.m_cmdClose.TabIndex = 7;
            this.m_cmdClose.Text = "Close";
            this.m_cmdClose.UseVisualStyleBackColor = true;
            this.m_cmdClose.Click += new System.EventHandler(this.OnCmdCloseClick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 222);
            this.Controls.Add(this.m_cmdClose);
            this.Controls.Add(this.m_cmdCallRead);
            this.Controls.Add(this.m_cmdCallPrintExtended);
            this.Controls.Add(this.m_lblMessageReceived);
            this.Controls.Add(this.m_txtMessageReceived);
            this.Controls.Add(this.m_txtMessageSent);
            this.Controls.Add(this.m_lblMessageSent);
            this.Controls.Add(this.m_cmdCallPrint);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(450, 250);
            this.Name = "MainWindow";
            this.Text = "WF4 Samples - Calling WF Service";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_cmdCallPrint;
        private System.Windows.Forms.Label m_lblMessageSent;
        private System.Windows.Forms.TextBox m_txtMessageSent;
        private System.Windows.Forms.TextBox m_txtMessageReceived;
        private System.Windows.Forms.Label m_lblMessageReceived;
        private System.Windows.Forms.Button m_cmdCallPrintExtended;
        private System.Windows.Forms.Button m_cmdCallRead;
        private System.Windows.Forms.Button m_cmdClose;
    }
}

