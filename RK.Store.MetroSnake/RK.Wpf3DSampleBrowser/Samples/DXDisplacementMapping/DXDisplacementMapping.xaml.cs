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
using RK.Common.GraphicsEngine.Drawing3D.Resources;
using RK.Common.GraphicsEngine.Objects;
using RK.Common.GraphicsEngine.Objects.Construction;

namespace RK.Wpf3DSampleBrowser.Samples.DXDisplacementMapping
{
    /// <summary>
    /// Interaction logic for DXDisplacementMapping.xaml
    /// </summary>
    [Export(Infrastructure.SAMPLE_CONTRACT, typeof(UserControl))]
    [Sample(SampleType.SharpDXSample, 2, "pack://application:,,,/RK.Wpf3DSampleBrowser;component/Resources/Icons/Planet32x32.png")]
    [DisplayName("Displacement Mapping")]
    public partial class DXDisplacementMapping : UserControl
    {
        private SimpleColoredDisplacedMaterialResource m_displacementMaterial;

        /// <summary>
        /// Initializes a new instance of the <see cref="DXDisplacementMapping" /> class.
        /// </summary>
        public DXDisplacementMapping()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;
        }

        /// <summary>
        /// Default load event - initializes all 3d content here.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CreateBackground();

            //Define needed textures
            m_direct3DImage.Resources.AddTexture("EarthTexture", new Uri("pack://application:,,,/RK.Wpf3DSampleBrowser;component/Resources/Textures/Earth.jpg"));
            m_direct3DImage.Resources.AddTexture("EarthHighMapTexture", new Uri("pack://application:,,,/RK.Wpf3DSampleBrowser;component/Resources/Textures/EarthHighMap.jpg"));

            //Create and and the displacement material
            m_displacementMaterial = new SimpleColoredDisplacedMaterialResource("EarthMaterial", "EarthHighMapTexture", "EarthTexture");
            m_displacementMaterial.DisplaceFactor = 1.1f;
            m_direct3DImage.Resources.AddResource(m_displacementMaterial);
            
            //Create and add the earth object
            VertexStructure vertexStructure = new VertexStructure();
            vertexStructure.BuildShpere(50, 50, 2f, Color4.White);
            vertexStructure.Material = "EarthMaterial";
            m_direct3DImage.Resources.AddGeometry("EarthGeometry", vertexStructure);
            m_direct3DImage.Scene.Add(new GenericObject("EarthGeometry"));

            //Configure the camera
            Camera camera = m_direct3DImage.Camera;
            camera.Position = new Vector3(0f, 1.5f, 0f);
            camera.RelativeTarget = new Common.Vector3(0f, 0f, 1f);
            camera.TargetRotation = new Vector2(camera.TargetRotation.X - 0.14f, -(float)Math.PI / 7f);
            camera.Zoom(-10f);
            camera.UpdateCamera();
        }

        /// <summary>
        /// Creates the fullscreen background object.
        /// </summary>
        private void CreateBackground()
        {
            m_direct3DImage.Resources.AddTexture("BackgroundTexture", new Uri("pack://application:,,,/RK.Wpf3DSampleBrowser;component/Resources/Textures/Background.png"));
            m_direct3DImage.Scene.Add(new TexturePainter("BackgroundTexture"));
        }

        private void OnCmdToggleWireframeClick(object sender, RoutedEventArgs e)
        {
            m_direct3DImage.IsWireframeEnabled = !m_direct3DImage.IsWireframeEnabled;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if ((this.ActualWidth > 0) &&
                (m_displacementMaterial != null))
            {
                double percentualMouseLocation = e.GetPosition(this).X / this.ActualWidth;
                m_displacementMaterial.DisplaceFactor = 1f * (float)percentualMouseLocation;
            }
        }
    }
}
