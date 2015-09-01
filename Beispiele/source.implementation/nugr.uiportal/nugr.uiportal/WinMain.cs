using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nugr.contract.application;
using nugr.contract.domainmodel;

namespace nugr.uiportal
{
    public partial class WinMain : Form
    {
        private IApplication _app;
        private CalcPrj _prj;


        public WinMain(IApplication app)
        {
            InitializeComponent();

            _app = app;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _prj = _app.Load(this.openFileDialog1.FileName);
                DisplayProject(_prj);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_prj != null)
                if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
                    _app.Save(_prj, this.saveFileDialog1.FileName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _prj = _app.Calculate(
                        long.Parse(this.textBox1.Text), 
                        long.Parse(this.textBox2.Text));

            DisplayProject(_prj);
        }

        private void DisplayProject(CalcPrj prj)
        {
            textBox1.Text = prj.From.ToString();
            textBox2.Text = prj.To.ToString();

            listBox1.Items.Clear();
            foreach(long p in prj.Primes)
                listBox1.Items.Add(p.ToString());
        }
    }
}
