using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using RK.Common;
using RK.Common.GraphicsEngine.Objects.Construction;

namespace RK.Common.GraphicsEngine.Objects.Wpf
{
    public class WpfGrid3DModel : WpfConstructed3DModel
    {
        public static readonly DependencyProperty TilesXProperty =
            DependencyProperty.Register("TilesX", typeof(int), typeof(WpfGrid3DModel), new PropertyMetadata(10));
        public static readonly DependencyProperty TilesZProperty =
            DependencyProperty.Register("TilesZ", typeof(int), typeof(WpfGrid3DModel), new PropertyMetadata(10));
        public static readonly DependencyProperty TileWidthProperty =
            DependencyProperty.Register("TileWidth", typeof(float), typeof(WpfGrid3DModel), new PropertyMetadata(1f));
        public static readonly DependencyProperty FloorBrushProperty =
            DependencyProperty.Register("FloorBrush", typeof(Brush), typeof(WpfGrid3DModel), new PropertyMetadata(Brushes.LightGray));
        public static readonly DependencyProperty StrokeBrushGroupLineProperty =
            DependencyProperty.Register("StrokeBrush", typeof(Brush), typeof(WpfGrid3DModel), new PropertyMetadata(Brushes.Black));
        public static readonly DependencyProperty StrokeBrushDefaultLineProperty =
            DependencyProperty.Register("StrokeBrushDefaultLine", typeof(Brush), typeof(WpfGrid3DModel), new PropertyMetadata(Brushes.Gray));
        public static readonly DependencyProperty GroupTileCountProperty =
            DependencyProperty.Register("GroupTileCount", typeof(int), typeof(WpfGrid3DModel), new PropertyMetadata(10));

        public Brush StrokeBrushDefaultLine
        {
            get { return (Brush)GetValue(StrokeBrushDefaultLineProperty); }
            set { SetValue(StrokeBrushDefaultLineProperty, value); }
        }

        public int GroupTileCount
        {
            get { return (int)GetValue(GroupTileCountProperty); }
            set { SetValue(GroupTileCountProperty, value); }
        }

        public Brush StrokeBrushGroupLine
        {
            get { return (Brush)GetValue(StrokeBrushGroupLineProperty); }
            set { SetValue(StrokeBrushGroupLineProperty, value); }
        }

        public Brush FloorBrush
        {
            get { return (Brush)GetValue(FloorBrushProperty); }
            set { SetValue(FloorBrushProperty, value); }
        }

        public float TileWidth
        {
            get { return (float)GetValue(TileWidthProperty); }
            set { SetValue(TileWidthProperty, value); }
        }

        public int TilesX
        {
            get { return (int)GetValue(TilesXProperty); }
            set { SetValue(TilesXProperty, value); }
        }

        public int TilesZ
        {
            get { return (int)GetValue(TilesZProperty); }
            set { SetValue(TilesZProperty, value); }
        }

        /// <summary>
        /// Builds the structures.
        /// </summary>
        public override VertexStructure[] BuildStructures()
        {
            //Calculate parameters
            Vector3 firstCoordinate = new Vector3(
                -TilesX / 2f,
                0f,
                -TilesZ / 2f);
            float tileWidthX = this.TileWidth;
            float tileWidthZ = this.TileWidth;
            float fieldWidth = tileWidthX * TilesX;
            float fieldDepth = tileWidthZ * TilesZ;
            float fieldWidthHalf = fieldWidth / 2f;
            float fieldDepthHalf = fieldDepth / 2f;

            //Define lower ground structure
            VertexStructure lowerGround = new VertexStructure();
            lowerGround.BuildRect4V(
                new Vector3(-fieldWidthHalf, -0.01f, -fieldDepthHalf),
                new Vector3(fieldWidthHalf, -0.01f, -fieldDepthHalf),
                new Vector3(fieldWidthHalf, -0.01f, fieldDepthHalf),
                new Vector3(-fieldWidthHalf, -0.01f, fieldDepthHalf));
            lowerGround.SetExtendedMaterialProperties(new WpfMaterialProperties() { WpfBrush = this.FloorBrush });

            //Define line structures
            VertexStructure genStructureDefaultLine = new VertexStructure();
            VertexStructure genStructureGroupLine = new VertexStructure();
            for (int actTileX = 0; actTileX < TilesX + 1; actTileX++)
            {
                Vector3 localStart = firstCoordinate + new Vector3(actTileX * tileWidthX, 0f, 0f);
                Vector3 localEnd = localStart + new Vector3(0f, 0f, tileWidthZ * TilesZ);

                VertexStructure targetStruture = actTileX % this.GroupTileCount == 0 ? genStructureGroupLine : genStructureDefaultLine;
                float devider = actTileX % this.GroupTileCount == 0 ? 25f : 100f;
                targetStruture.BuildRect4V(
                    localStart - new Vector3(tileWidthX / devider, 0f, 0f),
                    localStart + new Vector3(tileWidthX / devider, 0f, 0f),
                    localEnd + new Vector3(tileWidthX / devider, 0f, 0f),
                    localEnd - new Vector3(tileWidthX / devider, 0f, 0f));
            }
            for (int actTileZ = 0; actTileZ < TilesZ + 1; actTileZ++)
            {
                Vector3 localStart = firstCoordinate + new Vector3(0f, 0f, actTileZ * tileWidthZ);
                Vector3 localEnd = localStart + new Vector3(tileWidthX * TilesX, 0f, 0f);

                VertexStructure targetStruture = actTileZ % this.GroupTileCount == 0 ? genStructureGroupLine : genStructureDefaultLine;
                float devider = actTileZ % this.GroupTileCount == 0 ? 25f : 100f;
                targetStruture.BuildRect4V(
                    localStart + new Vector3(0f, 0f, tileWidthZ / devider),
                    localStart - new Vector3(0f, 0f, tileWidthZ / devider),
                    localEnd - new Vector3(0f, 0f, tileWidthZ / devider),
                    localEnd + new Vector3(0f, 0f, tileWidthZ / devider));
            }
            genStructureDefaultLine.SetExtendedMaterialProperties(new WpfMaterialProperties() { WpfBrush = this.StrokeBrushDefaultLine });
            genStructureGroupLine.SetExtendedMaterialProperties(new WpfMaterialProperties() { WpfBrush = this.StrokeBrushGroupLine });

            return new VertexStructure[] { lowerGround, genStructureDefaultLine, genStructureGroupLine };
        }
    }
}
