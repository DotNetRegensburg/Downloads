namespace Articles.Workflow.GuiCustomization
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.m_ctrlMainMenu = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_chkUseWorkflow = new System.Windows.Forms.ToolStripMenuItem();
            this.m_cmdReloadData = new System.Windows.Forms.ToolStripMenuItem();
            this.m_dataGrid = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_lblWFCallCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_lblWFTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_lblWFTimeAverage = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_dataSource = new System.Windows.Forms.BindingSource(this.components);
            this.m_colEmpty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceForGames = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceForHardware = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceForSoftware = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvailableMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sumDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_ctrlMainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_dataGrid)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_dataSource)).BeginInit();
            this.SuspendLayout();
            // 
            // m_ctrlMainMenu
            // 
            this.m_ctrlMainMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.m_ctrlMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.m_cmdReloadData});
            this.m_ctrlMainMenu.Location = new System.Drawing.Point(0, 0);
            this.m_ctrlMainMenu.Name = "m_ctrlMainMenu";
            this.m_ctrlMainMenu.Size = new System.Drawing.Size(837, 24);
            this.m_ctrlMainMenu.TabIndex = 0;
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_chkUseWorkflow});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // m_chkUseWorkflow
            // 
            this.m_chkUseWorkflow.Checked = true;
            this.m_chkUseWorkflow.CheckOnClick = true;
            this.m_chkUseWorkflow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_chkUseWorkflow.Name = "m_chkUseWorkflow";
            this.m_chkUseWorkflow.Size = new System.Drawing.Size(147, 22);
            this.m_chkUseWorkflow.Text = "Use Workflow";
            this.m_chkUseWorkflow.CheckedChanged += new System.EventHandler(this.OnChkUseWorkflowCheckedChanged);
            // 
            // m_cmdReloadData
            // 
            this.m_cmdReloadData.Image = global::Articles.Workflow.GuiCustomization.Properties.Resources.IconRefresh16x16;
            this.m_cmdReloadData.Name = "m_cmdReloadData";
            this.m_cmdReloadData.Size = new System.Drawing.Size(98, 20);
            this.m_cmdReloadData.Text = "Reload Data";
            this.m_cmdReloadData.Click += new System.EventHandler(this.OnCmdReloadDataClick);
            // 
            // m_dataGrid
            // 
            this.m_dataGrid.AllowUserToAddRows = false;
            this.m_dataGrid.AllowUserToDeleteRows = false;
            this.m_dataGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(249)))), ((int)(((byte)(251)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(145)))), ((int)(((byte)(216)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.m_dataGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.m_dataGrid.AutoGenerateColumns = false;
            this.m_dataGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.m_dataGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.m_dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_dataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_colEmpty,
            this.numberDataGridViewTextBoxColumn,
            this.customerIDDataGridViewTextBoxColumn,
            this.customerNameDataGridViewTextBoxColumn,
            this.PriceForGames,
            this.PriceForHardware,
            this.PriceForSoftware,
            this.AvailableMoney,
            this.sumDataGridViewTextBoxColumn});
            this.m_dataGrid.DataMember = "Deliveries";
            this.m_dataGrid.DataSource = this.m_dataSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(145)))), ((int)(((byte)(216)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dataGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.m_dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dataGrid.Location = new System.Drawing.Point(0, 24);
            this.m_dataGrid.MultiSelect = false;
            this.m_dataGrid.Name = "m_dataGrid";
            this.m_dataGrid.RowHeadersVisible = false;
            this.m_dataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dataGrid.ShowCellErrors = false;
            this.m_dataGrid.ShowCellToolTips = false;
            this.m_dataGrid.ShowEditingIcon = false;
            this.m_dataGrid.ShowRowErrors = false;
            this.m_dataGrid.Size = new System.Drawing.Size(837, 441);
            this.m_dataGrid.TabIndex = 12;
            this.m_dataGrid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.OnDataGridCellPainting);
            this.m_dataGrid.Paint += new System.Windows.Forms.PaintEventHandler(this.OnDataGridPaint);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.m_lblWFCallCount,
            this.toolStripStatusLabel2,
            this.m_lblWFTime,
            this.toolStripStatusLabel3,
            this.m_lblWFTimeAverage});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 465);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(837, 41);
            this.statusStrip1.TabIndex = 13;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(246, 32);
            this.toolStripStatusLabel1.Text = "Workflow Call Count: ";
            // 
            // m_lblWFCallCount
            // 
            this.m_lblWFCallCount.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblWFCallCount.Name = "m_lblWFCallCount";
            this.m_lblWFCallCount.Size = new System.Drawing.Size(29, 32);
            this.m_lblWFCallCount.Text = "0";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(137, 36);
            this.toolStripStatusLabel2.Text = "Total Time:";
            // 
            // m_lblWFTime
            // 
            this.m_lblWFTime.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblWFTime.Name = "m_lblWFTime";
            this.m_lblWFTime.Size = new System.Drawing.Size(62, 32);
            this.m_lblWFTime.Text = "0ms";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel3.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(259, 36);
            this.toolStripStatusLabel3.Text = "Average Time per Call:";
            // 
            // m_lblWFTimeAverage
            // 
            this.m_lblWFTimeAverage.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblWFTimeAverage.Name = "m_lblWFTimeAverage";
            this.m_lblWFTimeAverage.Size = new System.Drawing.Size(62, 32);
            this.m_lblWFTimeAverage.Text = "0ms";
            // 
            // m_dataSource
            // 
            this.m_dataSource.DataSource = typeof(Articles.Workflow.GuiCustomization.Data.DataCore);
            // 
            // m_colEmpty
            // 
            this.m_colEmpty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_colEmpty.HeaderText = "";
            this.m_colEmpty.Name = "m_colEmpty";
            // 
            // numberDataGridViewTextBoxColumn
            // 
            this.numberDataGridViewTextBoxColumn.DataPropertyName = "Number";
            this.numberDataGridViewTextBoxColumn.HeaderText = "Number";
            this.numberDataGridViewTextBoxColumn.Name = "numberDataGridViewTextBoxColumn";
            // 
            // customerIDDataGridViewTextBoxColumn
            // 
            this.customerIDDataGridViewTextBoxColumn.DataPropertyName = "CustomerID";
            this.customerIDDataGridViewTextBoxColumn.HeaderText = "CustomerID";
            this.customerIDDataGridViewTextBoxColumn.Name = "customerIDDataGridViewTextBoxColumn";
            // 
            // customerNameDataGridViewTextBoxColumn
            // 
            this.customerNameDataGridViewTextBoxColumn.DataPropertyName = "CustomerName";
            this.customerNameDataGridViewTextBoxColumn.HeaderText = "CustomerName";
            this.customerNameDataGridViewTextBoxColumn.Name = "customerNameDataGridViewTextBoxColumn";
            // 
            // PriceForGames
            // 
            this.PriceForGames.DataPropertyName = "PriceForGames";
            this.PriceForGames.HeaderText = "Games";
            this.PriceForGames.Name = "PriceForGames";
            // 
            // PriceForHardware
            // 
            this.PriceForHardware.DataPropertyName = "PriceForHardware";
            this.PriceForHardware.HeaderText = "Hardware";
            this.PriceForHardware.Name = "PriceForHardware";
            // 
            // PriceForSoftware
            // 
            this.PriceForSoftware.DataPropertyName = "PriceForSoftware";
            this.PriceForSoftware.HeaderText = "Software";
            this.PriceForSoftware.Name = "PriceForSoftware";
            // 
            // AvailableMoney
            // 
            this.AvailableMoney.DataPropertyName = "AvailableMoney";
            this.AvailableMoney.HeaderText = "Available Money";
            this.AvailableMoney.Name = "AvailableMoney";
            // 
            // sumDataGridViewTextBoxColumn
            // 
            this.sumDataGridViewTextBoxColumn.DataPropertyName = "Sum";
            this.sumDataGridViewTextBoxColumn.HeaderText = "Sum";
            this.sumDataGridViewTextBoxColumn.Name = "sumDataGridViewTextBoxColumn";
            this.sumDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 506);
            this.Controls.Add(this.m_dataGrid);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.m_ctrlMainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.m_ctrlMainMenu;
            this.Name = "MainWindow";
            this.Text = "WF4 Samples - Gui Customization";
            this.m_ctrlMainMenu.ResumeLayout(false);
            this.m_ctrlMainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_dataGrid)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_dataSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip m_ctrlMainMenu;
        private System.Windows.Forms.ToolStripMenuItem m_cmdReloadData;
        private System.Windows.Forms.DataGridView m_dataGrid;
        private System.Windows.Forms.BindingSource m_dataSource;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_chkUseWorkflow;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel m_lblWFCallCount;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel m_lblWFTime;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel m_lblWFTimeAverage;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_colEmpty;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PriceForGames;
        private System.Windows.Forms.DataGridViewTextBoxColumn PriceForHardware;
        private System.Windows.Forms.DataGridViewTextBoxColumn PriceForSoftware;
        private System.Windows.Forms.DataGridViewTextBoxColumn AvailableMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn sumDataGridViewTextBoxColumn;

    }
}

