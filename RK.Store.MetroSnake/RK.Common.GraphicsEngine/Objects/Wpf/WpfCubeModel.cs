using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using RK.Common.GraphicsEngine.Objects.Construction;

namespace RK.Common.GraphicsEngine.Objects.Wpf
{
    public class WpfCubeModel : WpfConstructed3DModel
    {
        public static readonly DependencyProperty CubeSurfaceProperty =
            DependencyProperty.Register("CubeSurface", typeof(Brush), typeof(WpfCubeModel), new PropertyMetadata(Brushes.White));

        public Brush CubeSurface
        {
            get { return (Brush)GetValue(CubeSurfaceProperty); }
            set { SetValue(CubeSurfaceProperty, value); }
        }

        /// <summary>
        /// Builds the structures.
        /// </summary>
        public override VertexStructure[] BuildStructures()
        {
            VertexStructure cubeStructure = new VertexStructure();

            cubeStructure.BuildCube24V(
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(1f, 1f, 1f),
                Color4.White);
            cubeStructure.SetExtendedMaterialProperties(new WpfMaterialProperties() 
            { 
                WpfBrush = CubeSurface,
            });

            return new VertexStructure[] { cubeStructure };
        }
    }
}
