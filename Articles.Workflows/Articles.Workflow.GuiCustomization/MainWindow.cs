using System;
using System.Activities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Articles.Workflow.GuiCustomization.Activities.Data;
using Articles.Workflow.GuiCustomization.Data;
using Articles.Workflow.GuiCustomization.Activities;

namespace Articles.Workflow.GuiCustomization
{
    public partial class MainWindow : Form
    {
        private int m_wfCallCount;
        private Stopwatch m_wfTime;
        private OverrideCellStyleActivity m_activityOverrideCellStyle;
        private WorkflowInvoker m_activityOverrideCellStyleInvoker;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            m_activityOverrideCellStyle = new OverrideCellStyleActivity();
            m_activityOverrideCellStyleInvoker = new WorkflowInvoker(m_activityOverrideCellStyle);

            m_wfCallCount = 0;
            m_wfTime = new Stopwatch();
        }

        /// <summary>
        /// Standard load event.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!DesignMode)
            {
                ReloadData();

                m_colEmpty.DisplayIndex = m_dataGrid.Columns.Count - 2;
            }
        }

        /// <summary>
        /// Reloads all displayed data.
        /// </summary>
        private void ReloadData()
        {
            Random randomizer = new Random(Environment.TickCount);
            int number = 0;

            //Build a dummy DataCore object
            DataCore dataCore = new DataCore();
            for (int loop = 0; loop < 2000; loop++)
            {
                dataCore.Deliveries.Add(new Delivery() 
                { 
                    CustomerID = randomizer.Next(4700, 9000).ToString().PadLeft(10, '0'), 
                    CustomerName = "Roland König", 
                    Number = (number++).ToString(),
                    PriceForGames = randomizer.Next(100, 300),
                    PriceForHardware = randomizer.Next(500, 900),
                    PriceForSoftware = randomizer.Next(50, 250),
                    AvailableMoney = randomizer.Next(800, 1200)
                });
            }

            m_dataSource.DataSource = dataCore;
        }

        /// <summary>
        /// Called when DataGrid is painting a cell.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellPaintingEventArgs"/> instance containing the event data.</param>
        private void OnDataGridCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) { return; }
                if (m_activityOverrideCellStyleInvoker == null) { return; }

                CustomCellStyle cellStyle = new CustomCellStyle();
                cellStyle.BackgroundColor = e.CellStyle.BackColor;
                cellStyle.ForegroundColor = e.CellStyle.ForeColor;

                Dictionary<string, object> inputs = new Dictionary<string, object>();
                inputs["ColumnName"] = m_dataGrid.Columns[e.ColumnIndex].DataPropertyName;
                inputs["DataRow"] = m_dataGrid.Rows[e.RowIndex].DataBoundItem;
                inputs["CellStyle"] = cellStyle;

                if (m_chkUseWorkflow.Checked)
                {
                    if (m_wfCallCount > 0) { m_wfTime.Start(); }
                    //m_activityOverrideCellStyleInvoker = new WorkflowInvoker(new OverrideCellStyleActivity());
                    m_activityOverrideCellStyleInvoker.Invoke(inputs);
                    if (m_wfCallCount > 0) { m_wfTime.Stop(); }

                    m_wfCallCount++;
                    m_lblWFCallCount.Text = m_wfCallCount.ToString();
                }
                else
                {
                    if (inputs["ColumnName"].ToString() == "Sum")
                    {
                        if (((Delivery)inputs["DataRow"]).Sum < 0)
                        {
                            cellStyle.BackgroundColor = Color.Red;
                            cellStyle.ForegroundColor = Color.Black;
                        }
                        else
                        {
                            cellStyle.BackgroundColor = Color.LightGreen;
                            cellStyle.ForegroundColor = Color.Black;
                        }
                    }
                }

                e.CellStyle.BackColor = cellStyle.BackgroundColor;
                e.CellStyle.ForeColor = cellStyle.ForegroundColor;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Called when user wants to enable/disable WorkflowFoundation usage.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnChkUseWorkflowCheckedChanged(object sender, EventArgs e)
        {
            m_dataGrid.Invalidate();
        }

        /// <summary>
        /// Called when user wants to reload all data.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnCmdReloadDataClick(object sender, EventArgs e)
        {
            ReloadData();
        }

        /// <summary>
        /// Called when datagrid is on painting itself.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        private void OnDataGridPaint(object sender, PaintEventArgs e)
        {
            m_lblWFTime.Text = m_wfTime.ElapsedMilliseconds.ToString() + "ms";
            m_lblWFTimeAverage.Text = ((double)m_wfTime.ElapsedMilliseconds / (double)m_wfCallCount).ToString("N2") + "ms";
        }
    }
}
