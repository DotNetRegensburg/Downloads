using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace RK.Wpf3DSampleBrowser.Samples.WpfSimpleCubes
{
    /// <summary>
    /// Interaction logic for WpfSimpleCubes.xaml
    /// </summary>
    [Export(Infrastructure.SAMPLE_CONTRACT, typeof(UserControl))]
    [Sample(SampleType.WpfSample, 2, "pack://application:,,,/RK.Wpf3DSampleBrowser;component/Resources/Icons/Cubes32x32.png")]
    [DisplayName("Simple Cubes")]
    public partial class WpfSimpleCubesControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WpfSimpleCubes" /> class.
        /// </summary>
        public WpfSimpleCubesControl()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;
        }

        /// <summary>
        /// Loads all models.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Material material = null;

            //Build blue cube (same like in Chapter2)
            material = new DiffuseMaterial(new SolidColorBrush(Colors.Blue));
            AddCube(new Point3D(-2, 1, -2), material);

            //Build a cube with a gradient
            material = new DiffuseMaterial(new LinearGradientBrush(
                Colors.Green, Colors.LightSteelBlue, 0));
            AddCube(new Point3D(-4, 1, 0), material);

            //Build a cube with a texture
            material = new DiffuseMaterial(new ImageBrush(new BitmapImage(
                new Uri("pack://application:,,,/Resources/Textures/Wall.jpg"))));
            AddCube(new Point3D(0, 1, -4), material);

            //Build a cube with a button on it
            material = new DiffuseMaterial(new VisualBrush(
                new Button() { Content = "Testbutton", Width = 256, Height = 256, FontSize = 50 }));
            AddCube(new Point3D(-2, 3, 0), material);

            //Build a cube with a datagrid on it
            material = new DiffuseMaterial(new VisualBrush(
                new DummyDataGridControl() { Width = 512, Height = 512 }));
            AddCube(new Point3D(0, 3, -2), material);
        }

        /// <summary>
        /// Adds a cube with the given material to the scene.
        /// </summary>
        /// <param name="position">Position of the cube.</param>
        /// <param name="material">Material for the cube.</param>
        private void AddCube(Point3D position, Material material)
        {
            //Create the mesh geometry
            MeshGeometry3D triangleMesh = new MeshGeometry3D();

            Point3D a = new Point3D(position.X - 1, position.Y - 1, position.Z + 1);
            Point3D b = new Point3D(position.X + 1, position.Y - 1, position.Z + 1);
            Point3D c = new Point3D(position.X + 1, position.Y - 1, position.Z - 1);
            Point3D d = new Point3D(position.X - 1, position.Y - 1, position.Z - 1);
            Point3D e = new Point3D(position.X - 1, position.Y + 1, position.Z + 1);
            Point3D f = new Point3D(position.X + 1, position.Y + 1, position.Z + 1);
            Point3D g = new Point3D(position.X + 1, position.Y + 1, position.Z - 1);
            Point3D h = new Point3D(position.X - 1, position.Y + 1, position.Z - 1);
            BuildRectangle(triangleMesh, a, b, f, e, new Vector3D(0, 0, 1));
            BuildRectangle(triangleMesh, b, c, g, f, new Vector3D(1, 0, 0));
            BuildRectangle(triangleMesh, c, d, h, g, new Vector3D(0, 0, -1));
            BuildRectangle(triangleMesh, d, a, e, h, new Vector3D(-1, 0, 0));
            BuildRectangle(triangleMesh, e, f, g, h, new Vector3D(0, 1, 0));
            BuildRectangle(triangleMesh, a, d, c, b, new Vector3D(0, -1, 0));

            //Build the model object
            GeometryModel3D triangleModel = new GeometryModel3D(
                triangleMesh,
                material);

            //Build the visual object
            ModelVisual3D model = new ModelVisual3D();
            model.Content = triangleModel;

            //Add the object to the viewport
            this.m_viewport.Visual3DChildren.Add(model);
        }

        /// <summary>
        /// Builds a rectangle using the given points a, b, c and d and the given normal.
        /// </summary>
        /// <param name="geometry">Target geometry.</param>
        /// <param name="a">Point a of the rectangle.</param>
        /// <param name="b">Point b of the rectangle.</param>
        /// <param name="c">Point c of the rectangle.</param>
        /// <param name="d">Point d of the rectangle.</param>
        /// <param name="normal">The normal.</param>
        private void BuildRectangle(
            MeshGeometry3D geometry,
            Point3D a, Point3D b, Point3D c, Point3D d,
            Vector3D normal)
        {
            int baseIndex = geometry.Positions.Count;

            //Add vertices
            geometry.Positions.Add(a);
            geometry.Positions.Add(b);
            geometry.Positions.Add(c);
            geometry.Positions.Add(d);

            //Add texture coordinates
            geometry.TextureCoordinates.Add(new Point(0, 1));
            geometry.TextureCoordinates.Add(new Point(1, 1));
            geometry.TextureCoordinates.Add(new Point(1, 0));
            geometry.TextureCoordinates.Add(new Point(0, 0));

            //Add normals
            geometry.Normals.Add(normal);
            geometry.Normals.Add(normal);
            geometry.Normals.Add(normal);
            geometry.Normals.Add(normal);

            //Add indices
            geometry.TriangleIndices.Add(baseIndex + 0);
            geometry.TriangleIndices.Add(baseIndex + 1);
            geometry.TriangleIndices.Add(baseIndex + 2);
            geometry.TriangleIndices.Add(baseIndex + 2);
            geometry.TriangleIndices.Add(baseIndex + 3);
            geometry.TriangleIndices.Add(baseIndex + 0);
        }
    }
}
