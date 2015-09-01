using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RK.Wpf3DSampleBrowser.Samples.WpfSimpleCubes
{
    /// <summary>
    /// Interaction logic for DummyDataGridControl.xaml
    /// </summary>
    public partial class DummyDataGridControl : UserControl
    {
        public DummyDataGridControl()
        {
            InitializeComponent();

            List<DummyDatacRow> dummyData = new List<DummyDatacRow>();
            for (int loop = 0; loop < 150; loop++)
            {
                dummyData.Add(new DummyDatacRow());
            }
            m_dataGrid.ItemsSource = dummyData;
        }
    }
}
