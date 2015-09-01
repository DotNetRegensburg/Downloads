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
using RK.Common;
using RK.Common.GraphicsEngine.Drawing3D;
using RK.Common.GraphicsEngine.Objects;

namespace RK.Wpf3DSampleBrowser.Samples.DXSimpleCubeDemo
{
    /// <summary>
    /// Interaction logic for DXSimpleCubeDemoControl.xaml
    /// </summary>
    [Export(Infrastructure.SAMPLE_CONTRACT, typeof(UserControl))]
    [Sample(SampleType.SharpDXSample, 1, "pack://application:,,,/RK.Wpf3DSampleBrowser;component/Resources/Icons/DXCube32x32.png")]
    [DisplayName("Simple Cube")]
    public partial class DXSimpleCubeDemoControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DXSimpleCubeDemoControl" /> class.
        /// </summary>
        public DXSimpleCubeDemoControl()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;
        }

        /// <summary>
        /// Initializes the 3d scene.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            m_direct3DImage.Scene.Add(new CubeObject());

            //Configure the camera
            Camera camera = m_direct3DImage.Camera;
            camera.Position = new Vector3(0f, 1.5f, 0f);
            camera.RelativeTarget = new Common.Vector3(0f, 0f, 1f);
            camera.TargetRotation = new Vector2(-(float)Math.PI / 4f, -(float)Math.PI / 7f);
            camera.Zoom(-10f);
            camera.UpdateCamera();
        }
    }
}
