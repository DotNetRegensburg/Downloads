using System;
using System.IO;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using RK.Common.GraphicsEngine.Objects.Construction;

namespace RK.Common.GraphicsEngine.Objects.Wpf
{
    public abstract class WpfConstructed3DModel : ModelVisual3D
    {
        public static readonly DependencyProperty FitToCenteredCuboidModeProperty =
            DependencyProperty.Register("FitToCenteredCuboidModeProperty", typeof(FitToCuboidMode), typeof(WpfConstructed3DModel), new PropertyMetadata(FitToCuboidMode.MaintainAspectRatio));
        public static readonly DependencyProperty FitToCenteredCuboidProperty =
            DependencyProperty.Register("FitToCenteredCuboid", typeof(bool), typeof(WpfConstructed3DModel), new PropertyMetadata(false));
        public static readonly DependencyProperty FitToCenteredCuboidOriginProperty =
            DependencyProperty.Register("FitToCenteredCuboidOrigin", typeof(FitToCuboidOrigin), typeof(WpfConstructed3DModel), new PropertyMetadata(FitToCuboidOrigin.Center));
        public static readonly DependencyProperty FitToCenteredCuboidWidthProperty =
            DependencyProperty.Register("FitToCenteredCuboidWidth", typeof(float), typeof(WpfConstructed3DModel), new PropertyMetadata(1f));
        public static readonly DependencyProperty FitToCenteredCuboidHeightProperty =
            DependencyProperty.Register("FitToCenteredCuboidHeight", typeof(float), typeof(WpfConstructed3DModel), new PropertyMetadata(1f));
        public static readonly DependencyProperty FitToCenteredCuboidDepthProperty =
            DependencyProperty.Register("FitToCenteredCuboidDepth", typeof(float), typeof(WpfConstructed3DModel), new PropertyMetadata(1f));

        private Model3DGroup m_modelGroup3D;

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfConstructed3DModel" /> class.
        /// </summary>
        public WpfConstructed3DModel()
        {
            m_modelGroup3D = new Model3DGroup();

            if (Application.Current != null)
            {
                //Trigger refresh of 3d models after property changes
                try
                {
                    Observable.FromEventPattern<DependencyPropertyChangedEventArgs>(this, "PropertyChanged")
                        .Where((eventPattern) => TriggersRefreshOf3DModel(eventPattern.EventArgs.Property))
                        .Throttle(TimeSpan.FromSeconds(0.5))
                        .ObserveOn(SynchronizationContext.Current)
                        .Subscribe((eventPattern) => Refresh3DModel());
                }
                catch { }
            }

            Refresh3DModel();
        }

        public event EventHandler<DependencyPropertyChangedEventArgs> PropertyChanged;

        public bool FitToCenteredCuboid
        {
            get { return (bool)GetValue(FitToCenteredCuboidProperty); }
            set { SetValue(FitToCenteredCuboidProperty, value); }
        }

        public FitToCuboidMode FitToCenteredCuboidMode
        {
            get { return (FitToCuboidMode)GetValue(FitToCenteredCuboidModeProperty); }
            set { SetValue(FitToCenteredCuboidModeProperty, value); }
        }

        public FitToCuboidOrigin FitToCenteredCuboidOrigin
        {
            get { return (FitToCuboidOrigin)GetValue(FitToCenteredCuboidOriginProperty); }
            set { SetValue(FitToCenteredCuboidOriginProperty, value); }
        }

        public float FitToCenteredCuboidWidth
        {
            get { return (float)GetValue(FitToCenteredCuboidWidthProperty); }
            set { SetValue(FitToCenteredCuboidWidthProperty, value); }
        }

        public float FitToCenteredCuboidHeight
        {
            get { return (float)GetValue(FitToCenteredCuboidHeightProperty); }
            set { SetValue(FitToCenteredCuboidHeightProperty, value); }
        }

        public float FitToCenteredCuboidDepth
        {
            get { return (float)GetValue(FitToCenteredCuboidDepthProperty); }
            set { SetValue(FitToCenteredCuboidDepthProperty, value); }
        }

        /// <summary>
        /// Builds the structures.
        /// </summary>
        /// <returns></returns>
        public abstract VertexStructure[] BuildStructures();

        /// <summary>
        /// Saves current 3d model to the given destination.
        /// </summary>
        /// <param name="modelSavePath">The path to save the model to.</param>
        public void SaveModelAsXamlTo(string modelSavePath)
        {
            Model3DGroup modelGroup = this.Content as Model3DGroup;
            if (modelGroup != null)
            {
                using (StreamWriter outStream = new StreamWriter(modelSavePath, false))
                {
                    XamlWriter.Save(modelGroup, outStream);
                }
            }
        }

        /// <summary>
        /// Refreshes the current 3d model.
        /// </summary>
        public void Refresh3DModel()
        {
            Model3DGroup generatedModelGroup = new Model3DGroup();

            //Get structures this model is based on
            VertexStructure[] loadedStructures = BuildStructures();
            if (loadedStructures == null)
            {
                this.Content = null;
                return;
            }

            if (this.FitToCenteredCuboid)
            {
                loadedStructures.FitToCenteredCuboid(
                    this.FitToCenteredCuboidWidth,
                    this.FitToCenteredCuboidHeight,
                    this.FitToCenteredCuboidDepth,
                    this.FitToCenteredCuboidMode,
                    this.FitToCenteredCuboidOrigin);
            }

            Material material = new DiffuseMaterial(new SolidColorBrush(Colors.LightGray));
            foreach (VertexStructure actStructure in loadedStructures)
            {
                MeshGeometry3D triangleMesh = new MeshGeometry3D();
                foreach (Vertex actVertex in actStructure.Vertices)
                {
                    triangleMesh.Positions.Add(new Point3D((double)actVertex.Position.X, (double)actVertex.Position.Y, (double)actVertex.Position.Z));
                    triangleMesh.Normals.Add(new Vector3D((double)actVertex.Normal.X, (double)actVertex.Normal.Y, (double)actVertex.Normal.Z));
                    triangleMesh.TextureCoordinates.Add(new Point((double)actVertex.TexCoord.X, (double)actVertex.TexCoord.Y));
                }
                foreach (Triangle actTrianlge in actStructure.Triangles)
                {
                    triangleMesh.TriangleIndices.Add((int)actTrianlge.Index1);
                    triangleMesh.TriangleIndices.Add((int)actTrianlge.Index2);
                    triangleMesh.TriangleIndices.Add((int)actTrianlge.Index3);
                }

                Material wpfMaterial = CreateWpfMaterial(actStructure);
                GeometryModel3D geometryModel = new GeometryModel3D(triangleMesh, wpfMaterial);

                generatedModelGroup.Children.Add(geometryModel);
            }

            this.Content = generatedModelGroup;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (PropertyChanged != null) { PropertyChanged(this, e); }
        }

        /// <summary>
        /// Refreshes local 3D model when parent changes.
        /// </summary>
        /// <param name="oldParent">The old parent object.</param>
        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);

            Refresh3DModel();
        }

        /// <summary>
        /// Does a change of the given property trigger the refresh of local geometry?
        /// </summary>
        /// <param name="property">The changed property</param>
        protected virtual bool TriggersRefreshOf3DModel(DependencyProperty property)
        {
            if (property.OwnerType.IsSubclassOf(typeof(WpfConstructed3DModel))) { return true; }
            if (property.OwnerType == typeof(WpfConstructed3DModel)) { return true; }

            return false;
        }

        private Material CreateWpfMaterial(VertexStructure vertexStructure)
        {
            WpfMaterialProperties wpfMaterialProperties = vertexStructure.GetExtendedMaterialProperties<WpfMaterialProperties>();
            if (wpfMaterialProperties != null)
            {
                MaterialGroup materialGroup = new MaterialGroup();
                materialGroup.Children.Add(new DiffuseMaterial(wpfMaterialProperties.WpfBrush) { AmbientColor = vertexStructure.MaterialProperties.AmbientColor.ToWpfColor() });
                materialGroup.Children.Add(new EmissiveMaterial(new SolidColorBrush(vertexStructure.MaterialProperties.EmissiveColor.ToWpfColor())));
                materialGroup.Children.Add(new SpecularMaterial(new SolidColorBrush(vertexStructure.MaterialProperties.Specular.ToWpfColor()), (double)vertexStructure.MaterialProperties.Shininess));

                return materialGroup;
            }

            return new DiffuseMaterial(new SolidColorBrush(vertexStructure.MaterialProperties.DiffuseColor.ToWpfColor()));
        }
    }
}
