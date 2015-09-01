namespace Articles.Workflow.CustomWorkflowService
{
    partial class ServerHostWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_cmdClose = new System.Windows.Forms.Button();
            this.m_lblHostAddress = new System.Windows.Forms.Label();
            this.m_txtUrl = new System.Windows.Forms.LinkLabel();
            this.m_webBrowserService = new System.Windows.Forms.WebBrowser();
            this.m_tabControl = new System.Windows.Forms.TabControl();
            this.m_tabPageService = new System.Windows.Forms.TabPage();
            this.m_tabPageWsdl = new System.Windows.Forms.TabPage();
            this.m_webBrowserWsdl = new System.Windows.Forms.WebBrowser();
            this.m_tabControl.SuspendLayout();
            this.m_tabPageService.SuspendLayout();
            this.m_tabPageWsdl.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_cmdClose
            // 
            this.m_cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cmdClose.Location = new System.Drawing.Point(516, 359);
            this.m_cmdClose.Name = "m_cmdClose";
            this.m_cmdClose.Size = new System.Drawing.Size(131, 23);
            this.m_cmdClose.TabIndex = 0;
            this.m_cmdClose.Text = "Close";
            this.m_cmdClose.UseVisualStyleBackColor = true;
            this.m_cmdClose.Click += new System.EventHandler(this.OnCmdCloseClick);
            // 
            // m_lblHostAddress
            // 
            this.m_lblHostAddress.AutoSize = true;
            this.m_lblHostAddress.Location = new System.Drawing.Point(12, 15);
            this.m_lblHostAddress.Name = "m_lblHostAddress";
            this.m_lblHostAddress.Size = new System.Drawing.Size(23, 13);
            this.m_lblHostAddress.TabIndex = 2;
            this.m_lblHostAddress.Text = "Url:";
            // 
            // m_txtUrl
            // 
            this.m_txtUrl.AutoSize = true;
            this.m_txtUrl.Location = new System.Drawing.Point(84, 15);
            this.m_txtUrl.Name = "m_txtUrl";
            this.m_txtUrl.Size = new System.Drawing.Size(55, 13);
            this.m_txtUrl.TabIndex = 3;
            this.m_txtUrl.TabStop = true;
            this.m_txtUrl.Text = "linkLabel1";
            this.m_txtUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnTxtUrlClick);
            // 
            // m_webBrowserService
            // 
            this.m_webBrowserService.AllowNavigation = false;
            this.m_webBrowserService.AllowWebBrowserDrop = false;
            this.m_webBrowserService.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_webBrowserService.Location = new System.Drawing.Point(3, 3);
            this.m_webBrowserService.MinimumSize = new System.Drawing.Size(20, 20);
            this.m_webBrowserService.Name = "m_webBrowserService";
            this.m_webBrowserService.ScriptErrorsSuppressed = true;
            this.m_webBrowserService.Size = new System.Drawing.Size(621, 280);
            this.m_webBrowserService.TabIndex = 4;
            // 
            // m_tabControl
            // 
            this.m_tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tabControl.Controls.Add(this.m_tabPageService);
            this.m_tabControl.Controls.Add(this.m_tabPageWsdl);
            this.m_tabControl.Location = new System.Drawing.Point(12, 41);
            this.m_tabControl.Name = "m_tabControl";
            this.m_tabControl.SelectedIndex = 0;
            this.m_tabControl.Size = new System.Drawing.Size(635, 312);
            this.m_tabControl.TabIndex = 5;
            // 
            // m_tabPageService
            // 
            this.m_tabPageService.Controls.Add(this.m_webBrowserService);
            this.m_tabPageService.Location = new System.Drawing.Point(4, 22);
            this.m_tabPageService.Name = "m_tabPageService";
            this.m_tabPageService.Padding = new System.Windows.Forms.Padding(3);
            this.m_tabPageService.Size = new System.Drawing.Size(627, 286);
            this.m_tabPageService.TabIndex = 0;
            this.m_tabPageService.Text = "Service";
            this.m_tabPageService.UseVisualStyleBackColor = true;
            // 
            // m_tabPageWsdl
            // 
            this.m_tabPageWsdl.Controls.Add(this.m_webBrowserWsdl);
            this.m_tabPageWsdl.Location = new System.Drawing.Point(4, 22);
            this.m_tabPageWsdl.Name = "m_tabPageWsdl";
            this.m_tabPageWsdl.Padding = new System.Windows.Forms.Padding(3);
            this.m_tabPageWsdl.Size = new System.Drawing.Size(627, 286);
            this.m_tabPageWsdl.TabIndex = 1;
            this.m_tabPageWsdl.Text = "Wsdl";
            this.m_tabPageWsdl.UseVisualStyleBackColor = true;
            // 
            // m_webBrowserWsdl
            // 
            this.m_webBrowserWsdl.AllowNavigation = false;
            this.m_webBrowserWsdl.AllowWebBrowserDrop = false;
            this.m_webBrowserWsdl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_webBrowserWsdl.Location = new System.Drawing.Point(3, 3);
            this.m_webBrowserWsdl.MinimumSize = new System.Drawing.Size(20, 20);
            this.m_webBrowserWsdl.Name = "m_webBrowserWsdl";
            this.m_webBrowserWsdl.ScriptErrorsSuppressed = true;
            this.m_webBrowserWsdl.Size = new System.Drawing.Size(621, 280);
            this.m_webBrowserWsdl.TabIndex = 5;
            // 
            // ServerHostWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 394);
            this.Controls.Add(this.m_tabControl);
            this.Controls.Add(this.m_txtUrl);
            this.Controls.Add(this.m_lblHostAddress);
            this.Controls.Add(this.m_cmdClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServerHostWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Custom Workflow Server";
            this.m_tabControl.ResumeLayout(false);
            this.m_tabPageService.ResumeLayout(false);
            this.m_tabPageWsdl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_cmdClose;
        private System.Windows.Forms.Label m_lblHostAddress;
        private System.Windows.Forms.LinkLabel m_txtUrl;
        private System.Windows.Forms.WebBrowser m_webBrowserService;
        private System.Windows.Forms.TabControl m_tabControl;
        private System.Windows.Forms.TabPage m_tabPageService;
        private System.Windows.Forms.TabPage m_tabPageWsdl;
        private System.Windows.Forms.WebBrowser m_webBrowserWsdl;
    }
}