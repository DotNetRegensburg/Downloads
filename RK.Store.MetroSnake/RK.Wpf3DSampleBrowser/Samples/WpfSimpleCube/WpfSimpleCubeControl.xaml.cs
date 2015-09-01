using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
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

namespace RK.Wpf3DSampleBrowser.Samples.WpfSimpleCube
{
    /// <summary>
    /// Interaction logic for WpfSimpleCubeControl.xaml
    /// </summary>
    [Export(Infrastructure.SAMPLE_CONTRACT, typeof(UserControl))]
    [Sample(SampleType.WpfSample, 1, "pack://application:,,,/RK.Wpf3DSampleBrowser;component/Resources/Icons/Cube32x32.png")]
    [DisplayName("Simple Cube")]
    public partial class WpfSimpleCubeControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WpfSimpleCubeControl" /> class.
        /// </summary>
        public WpfSimpleCubeControl()
        {
            InitializeComponent();
        }
    }
}
