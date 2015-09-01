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

namespace RK.Common.GraphicsEngine.Objects.ObjectTypes
{
    public class Grid3DType : ObjectType
    {
        public string LineMaterial
        {
            get;
            set;
        }

        public string GroupLineMaterial
        {
            get;
            set;
        }

        public string GroundMaterial
        {
            get;
            set;
        }

        public int GroupTileCount
        {
            get;
            set;
        }

        public Color4 LineColor
        {
            get;
            set;
        }

        public Color4 GroundColor
        {
            get;
            set;
        }

        public float TileWidth
        {
            get;
            set;
        }

        public int TilesX
        {
            get;
            set;
        }

        public int TilesZ
        {
            get;
            set;
        }

        /// <summary>
        /// Builds the structures.
        /// </summary>
        public override VertexStructure[] BuildStructure()
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
                new Vector3(-fieldWidthHalf, -0.01f, fieldDepthHalf),
                this.GroundColor);
            lowerGround.Material = this.GroundMaterial;

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
                    localEnd - new Vector3(tileWidthX / devider, 0f, 0f),
                    this.LineColor);
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
                    localEnd + new Vector3(0f, 0f, tileWidthZ / devider),
                    this.LineColor);
            }
            genStructureDefaultLine.Material = this.LineMaterial;
            genStructureGroupLine.Material = this.LineMaterial;

            return new VertexStructure[] { lowerGround, genStructureDefaultLine, genStructureGroupLine };
        }
    }
}
