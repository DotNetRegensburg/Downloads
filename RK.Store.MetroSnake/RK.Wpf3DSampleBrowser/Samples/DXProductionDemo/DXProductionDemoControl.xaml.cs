using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
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
using System.Windows.Resources;
using System.Windows.Shapes;
using RK.Common;
using RK.Common.GraphicsEngine.Drawing3D;
using RK.Common.GraphicsEngine.Drawing3D.Resources;
using RK.Common.GraphicsEngine.Objects;
using RK.Common.GraphicsEngine.Objects.Construction;
using RK.Common.GraphicsEngine.Objects.ObjectTypes;
using RK.Common.GraphicsEngine.Animations;

namespace RK.Wpf3DSampleBrowser.Samples.DXProductionDemo
{
    /// <summary>
    /// Interaction logic for DXSimpleScreenControl.xaml
    /// </summary>
    [Export(Infrastructure.SAMPLE_CONTRACT, typeof(UserControl))]
    [Sample(SampleType.SharpDXSample, 3, "pack://application:,,,/RK.Wpf3DSampleBrowser;component/Resources/Icons/Production32x32.png")]
    [DisplayName("Production")]
    public partial class DXProductionDemoControl : UserControl
    {
        private List<BottleSource> m_bottleSources;
        private bool m_productionEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="DXSimpleScreenControl" /> class.
        /// </summary>
        public DXProductionDemoControl()
        {
            InitializeComponent();

            m_bottleSources = new List<BottleSource>();

            this.Loaded += OnLoaded;

            //Start the production loop
            m_productionEnabled = true;
            this.InvokeDelayedWhileWpf(
                () => true,
                () => ProduceBottles(),
                TimeSpan.FromSeconds(0.3));
        }

        /// <summary>
        /// A method that control bottle creation and animation.
        /// </summary>
        private void ProduceBottles()
        {
            if ((this.Parent != null) && m_productionEnabled)
            {
                foreach (BottleSource actBottleSource in m_bottleSources)
                {
                    //Create the bottle object
                    GenericObject newBottle = new GenericObject("BottleGeometry");
                    newBottle.Position = actBottleSource.SourceLocation;
                    newBottle.Scaling = new Vector3(0.5f, 0.5f, 0.5f);
                    m_direct3DImage.Scene.Add(newBottle);

                    //Define the animation
                    newBottle.BeginAnimationSequence()
                        .Move3D(actBottleSource.TargetLocation - actBottleSource.SourceLocation, TimeSpan.FromSeconds(12.0))
                        .WaitUnitTimePassed(TimeSpan.FromSeconds(5.5))
                        .CallAction(() => newBottle.ChangeGeometry("BottleGreenGeometry"))
                        .WaitFinished()
                        .Finish(() => m_direct3DImage.Scene.Remove(newBottle));
                }
            }
        }

        /// <summary>
        /// Standard load event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //Create all resources and main objects
            CreateBackground();
            CreateFloor(new Vector2(3f, 3f), 14, 15);
            CreateMachineResources();
            CreateConveyorResources(24);
            CreateBottleResources();

            //Create all machine lines
            CreateMachineLine(new Vector3(0f, 0f, 0f));
            CreateMachineLine(new Vector3(0f, 0f, 5f));
            CreateMachineLine(new Vector3(0f, 0f, 10f));
            CreateMachineLine(new Vector3(0f, 0f, 15f));
            CreateMachineLine(new Vector3(0f, 0f, 20f));
            CreateMachineLine(new Vector3(0f, 0f, -5f));
            CreateMachineLine(new Vector3(0f, 0f, -10f));
            CreateMachineLine(new Vector3(0f, 0f, -15f));
            CreateMachineLine(new Vector3(0f, 0f, -20f));

            //Configure the camera
            Camera camera = m_direct3DImage.Camera;
            camera.Position = new Vector3(0f, 1.5f, 0f);
            camera.RelativeTarget = new Common.Vector3(0f, 0f, 1f);
            camera.TargetRotation = new Vector2(camera.TargetRotation.X - 0.14f, -(float)Math.PI / 7f);
            camera.Zoom(-10f);
            camera.UpdateCamera();
        }

        /// <summary>
        /// Creates a new machine line with the given center location.
        /// </summary>
        /// <param name="centerLocation">The center of the generated machine line.</param>
        private void CreateMachineLine(Vector3 centerLocation)
        {
            //Instanciate objects
            m_direct3DImage.Scene.Add(new GenericObject("MachineGeometry", centerLocation + new Vector3(0f, 0f, 0f)));
            m_direct3DImage.Scene.Add(new GenericObject("MachineGeometry", centerLocation + new Vector3(-15f, 0f, 0f)));
            m_direct3DImage.Scene.Add(new GenericObject("MachineGeometry", centerLocation + new Vector3(15f, 0f, 0f)));
            m_direct3DImage.Scene.Add(new GenericObject("ConveyorGeometry", centerLocation + new Vector3(-13f, 0f, -0.5f)));

            //Create bottle sources
            m_bottleSources.Add(new BottleSource()
            {
                SourceLocation = centerLocation + new Vector3(-13f, 0.7f, 0f),
                TargetLocation = centerLocation + new Vector3(15f, 0.7f, 0f)
            });
        }

        /// <summary>
        /// Creates the fullscreen background object.
        /// </summary>
        private void CreateBackground()
        {
            m_direct3DImage.Resources.AddTexture("BackgroundTexture", new Uri("pack://application:,,,/RK.Wpf3DSampleBrowser;component/Resources/Textures/Background.png"));
            m_direct3DImage.Scene.Add(new TexturePainter("BackgroundTexture"));
        }

        /// <summary>
        /// Creates the floor 
        /// </summary>
        /// <param name="tileSize"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void CreateFloor(Vector2 tileSize, int width, int height)
        {
            m_direct3DImage.Resources.AddTexture("FloorTexture", new Uri("pack://application:,,,/RK.Wpf3DSampleBrowser;component/Resources/Textures/Floor.png"));
            m_direct3DImage.Resources.AddSimpleTexturedMaterial("FloorMaterial", "FloorTexture");
            m_direct3DImage.Resources.AddSimpleColoredMaterial("FloorSideMaterial");

            FloorType FloorType = new FloorType(tileSize, 0f);
            FloorType.BorderMaterial = "FloorMaterial";
            FloorType.BottomMaterial = "FloorMaterial";
            FloorType.DefaultFloorMaterial = "FloorMaterial";
            FloorType.SideMaterial = "FloorSideMaterial";
            FloorType.SetTilemap(width, height);

            m_direct3DImage.Resources.AddGeometry("FloorGeometry", FloorType);
            m_direct3DImage.Scene.Add(new GenericObject("FloorGeometry"));
        }

        /// <summary>
        /// Creates all 3D resources for a machine.
        /// </summary>
        private void CreateMachineResources()
        {
            //Define materials for the machine
            m_direct3DImage.Resources.AddResource<TextureResource>(new LinearGradientTextureResource(
                "MachineMainTexture",
                Color4.White, Color4.LightSteelBlue,
                GradientDirection.Directional));
            m_direct3DImage.Resources.AddResource<TextureResource>(new LinearGradientTextureResource(
                "MachineColumnTexture",
                Color4.LightGray, Color4.DimGray,
                GradientDirection.Directional));
            m_direct3DImage.Resources.AddSimpleTexturedMaterial("MachineMainMaterial", "MachineMainTexture");
            m_direct3DImage.Resources.AddSimpleTexturedMaterial("MachineColumnMaterial", "MachineColumnTexture");

            //Define geometry for the machine
            SimpleMachineType machineType = new SimpleMachineType();
            machineType.ColumnMaterial = "MachineColumnMaterial";
            machineType.MachineMaterial = "MachineMainMaterial";
            m_direct3DImage.Resources.AddGeometry("MachineGeometry", machineType);
        }

        /// <summary>
        /// Create geometry needed for bottles.
        /// </summary>
        private void CreateBottleResources()
        {
            m_direct3DImage.Resources.AddGeometry(
                "BottleGeometry",
                ObjectType.FromACFile(new Uri("pack://application:,,,/RK.Wpf3DSampleBrowser;component/Resources/Models/Bottle.ac")));
            m_direct3DImage.Resources.AddGeometry(
                "BottleGreenGeometry",
                ObjectType.FromACFile(new Uri("pack://application:,,,/RK.Wpf3DSampleBrowser;component/Resources/Models/BottleGreen.ac")));
        }

        /// <summary>
        /// Creates all conveyor resources.
        /// </summary>
        /// <param name="scene">Scene where to create all the resources.</param>
        private void CreateConveyorResources(int binsX)
        {
            int binsY = 2;
            VertexStructure[] highrackStructures = new VertexStructure[3];

            //Build all ground's
            highrackStructures[0] = new VertexStructure();
            highrackStructures[1] = new VertexStructure();
            for (int loop = 1; loop < binsY; loop++)
            {
                highrackStructures[0].BuildCubeBottom4V(
                    new Vector3(0, loop * 0.6f, 0),
                    new Vector3(binsX * 1.125f, 0.1f, 1f),
                    Color4.LightSteelBlue);
                highrackStructures[0].BuildCubeSides16V(
                    new Vector3(0, loop * 0.6f, 0),
                    new Vector3(binsX * 1.125f, 0.1f, 1f),
                    Color4.LightSteelBlue);
                highrackStructures[0].Material = "ConveyorSideMaterial";

                highrackStructures[1].EnableTextureTileMode(new Vector2(1f, 1f));
                highrackStructures[1].BuildCubeTop4V(
                    new Vector3(0, loop * 0.6f, 0),
                    new Vector3(binsX * 1.125f, 0.1f, 1f),
                    Color4.LightSteelBlue);
                highrackStructures[1].Material = "ConveyorTopMaterial";
            }

            //Build all columns
            highrackStructures[2] = new VertexStructure();
            for (int loop = 0; loop < binsX + 1; loop++)
            {
                highrackStructures[2].BuildCube24V(
                    new Vector3(loop * 1.125f, 0f, 0f),
                    0.1f, (binsY - 1f) * 0.6f + 0.12f,
                    Color4.SteelBlue);
                highrackStructures[2].BuildCube24V(
                    new Vector3(loop * 1.125f, 0f, 1f),
                    0.1f, (binsY - 1f) * 0.6f + 0.12f,
                    Color4.SteelBlue);
                highrackStructures[2].Material = "ConveyorColumnMaterial";
            }

            //Build resources
            ObjectType rackType = new GenericObjectType(highrackStructures);
            m_direct3DImage.Resources.AddResource(new GeometryResource("ConveyorGeometry", rackType));
            m_direct3DImage.Resources.AddResource<TextureResource>(new LinearGradientTextureResource(
                "ConveyorColumnTexture",
                Color4.LightGray, Color4.DimGray,
                GradientDirection.Directional));
            m_direct3DImage.Resources.AddTexture("ConveyorTopTexture", new Uri("pack://application:,,,/RK.Wpf3DSampleBrowser;component/Resources/Textures/ConveyorGround.png"));
            m_direct3DImage.Resources.AddResource(new SimpleColoredMaterialResource("ConveyorSideMaterial"));
            m_direct3DImage.Resources.AddResource(new SimpleColoredMaterialResource("ConveyorTopMaterial", "ConveyorTopTexture"));
            m_direct3DImage.Resources.AddResource(new SimpleColoredMaterialResource("ConveyorColumnMaterial", "ConveyorColumnTexture"));
        }

        private void OnCmdToggleWireframeClick(object sender, RoutedEventArgs e)
        {
            m_direct3DImage.IsWireframeEnabled = !m_direct3DImage.IsWireframeEnabled;
        }

        private void OnCmdToggleProductionClick(object sender, RoutedEventArgs e)
        {
            m_productionEnabled = !m_productionEnabled;
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        private class BottleSource
        {
            public Vector3 SourceLocation;
            public Vector3 TargetLocation;
        }
    }
}
