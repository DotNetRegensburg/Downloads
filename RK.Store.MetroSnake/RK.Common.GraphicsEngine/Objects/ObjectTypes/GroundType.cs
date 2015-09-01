using System;
using System.Collections.Generic;
using RK.Common.GraphicsEngine.Objects.Construction;

namespace RK.Common.GraphicsEngine.Objects.ObjectTypes
{
    public class FloorType : ObjectType
    {
        public const float DEFAULT_HEIGHT = 0.1f;

        //Properties
        private float m_borderSize;
        private Vector2 m_totalSizeWithoutBorder;
        private Vector2 m_tileSize;
        private float m_height;
        private int m_tilesX;
        private int m_tilesY;
        private List<GroundTile> m_groundTiles;
        private List<BorderInformation> m_borders;

        //Material names
        private string m_borderMaterial;
        private string m_groundMaterial;
        private string m_sideMaterial;
        private string m_bottomMaterial;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroundObject"/> class.
        /// </summary>
        public FloorType(Vector2 tileSize, float borderSize)
        {
            m_height = DEFAULT_HEIGHT;
            m_groundTiles = new List<GroundTile>();
            m_borders = new List<BorderInformation>();
            m_tileSize = tileSize;
            m_borderSize = borderSize;
        }

        /// <summary>
        /// Sets the tilemap.
        /// </summary>
        /// <param name="tileMap">The new tilemap to apply (use null for empty tiles).</param>
        public void SetTilemap(GroundTileInfo[,] tileMap)
        {
            //Get width and height of the tilemap
            int tilesX = tileMap.GetLength(0);
            int tilesY = tileMap.GetLength(1);
            if (tilesX <= 0) { throw new ArgumentException("Width of tilemap <= 0!", "tileMap"); }
            if (tilesY <= 0) { throw new ArgumentException("Height of tilemap <= 0!", "tileMap"); }
            m_tilesX = tilesX;
            m_tilesY = tilesY;

            //Update total size
            m_totalSizeWithoutBorder = new Vector2(
                tilesX * m_tileSize.X,
                tilesY * m_tileSize.Y);

            //Generate all tiles
            m_groundTiles.Clear();
            for (int loopX = 0; loopX < tilesX; loopX++)
            {
                for (int loopY = 0; loopY < tilesY; loopY++)
                {
                    if (tileMap[loopX, loopY] != null)
                    {
                        GroundTile newTile = new GroundTile(loopX, loopY, tileMap[loopX, loopY]);
                        m_groundTiles.Add(newTile);
                    }
                }
            }

            //Generate all borders
            bool[,] boolTileMap = CreateBooleanMap(tileMap);
            GenerateBorders(boolTileMap, tilesX, tilesY);
        }

        /// <summary>
        /// Sets a tilemap using the given width and height and sets all tiles to default resource.
        /// </summary>
        /// <param name="width">Width of the tilemap.</param>
        /// <param name="height">Height of the tilemap.</param>
        public void SetTilemap(int width, int height)
        {
            bool[,] tilemap = new bool[width, height];
            for (int loopX = 0; loopX < width; loopX++)
            {
                for (int loopY = 0; loopY < height; loopY++)
                {
                    tilemap[loopX, loopY] = true;
                }
            }
            SetTilemap(tilemap);
        }

        /// <summary>
        /// Sets the tilemap.
        /// </summary>
        /// <param name="timeMap">The new tilemap to apply (use null for empty tiles).</param>
        public void SetTilemap(bool[,] tileMap)
        {
            //Get width and height of the tilemap
            int tilesX = tileMap.GetLength(0);
            int tilesY = tileMap.GetLength(1);
            if (tilesX <= 0) { throw new ArgumentException("Width of tilemap <= 0!", "tileMap"); }
            if (tilesY <= 0) { throw new ArgumentException("Height of tilemap <= 0!", "tileMap"); }
            m_tilesX = tilesX;
            m_tilesY = tilesY;

            //Update total size
            m_totalSizeWithoutBorder = new Vector2(
                tilesX * m_tileSize.X,
                tilesY * m_tileSize.Y);

            //Generate all tiles
            m_groundTiles.Clear();
            for (int loopX = 0; loopX < tilesX; loopX++)
            {
                for (int loopY = 0; loopY < tilesY; loopY++)
                {
                    if (tileMap[loopX, loopY])
                    {
                        GroundTile newTile = new GroundTile(loopX, loopY);
                        m_groundTiles.Add(newTile);
                    }
                }
            }

            //Generate all borders
            GenerateBorders(tileMap, tilesX, tilesY);
        }

        /// <summary>
        /// Gets the center coordinate of the tile at the given location.
        /// </summary>
        /// <param name="xPos">X position of the requested tile.</param>
        /// <param name="yPos">Y position of the requested tile.</param>
        public Vector3 GetTilePosition(int xPos, int yPos)
        {
            //Check parameters
            if ((xPos < 0) || (xPos >= m_tilesX)) { throw new ArgumentException("Invalid x position!", "xPos"); }
            if ((yPos < 0) || (yPos >= m_tilesY)) { throw new ArgumentException("Invalid y position!", "yPos"); }

            //Calculate half sizes
            Vector2 totalHalfSize = new Vector2(m_totalSizeWithoutBorder.X / 2f, m_totalSizeWithoutBorder.Y / 2f);
            Vector2 tileHalfSize = new Vector2(m_tileSize.X / 2f, m_tileSize.Y / 2f);

            //Get position of the tile
            return new Vector3(
                (xPos * m_tileSize.X) - totalHalfSize.X + tileHalfSize.X,
                0f,
                (yPos * m_tileSize.Y) - totalHalfSize.Y + tileHalfSize.Y);
        }

        /// <summary>
        /// Builds the structure.
        /// </summary>
        public override VertexStructure[] BuildStructure()
        {
            List<VertexStructure> result = new List<VertexStructure>();

            //Hold dictionary containg materials and corresponding structures
            Dictionary<string, VertexStructure> materialRelated = new Dictionary<string, VertexStructure>();
            
            //Build bottom structure
            VertexStructure bottomStructure = new VertexStructure();
            bottomStructure.Material = m_bottomMaterial;
            materialRelated[m_bottomMaterial] = bottomStructure;
            result.Add(bottomStructure);

            //Calculate half vector of total ground size.
            Vector2 totalHalfSize = new Vector2(m_totalSizeWithoutBorder.X / 2f, m_totalSizeWithoutBorder.Y / 2f);
            Vector2 tileHalfSize = new Vector2(m_tileSize.X / 2f, m_tileSize.Y / 2f);

            //Build all tiles
            foreach (GroundTile actTile in m_groundTiles)
            {
                //Get the material of the tile
                string actMaterial = actTile.Material;
                if (string.IsNullOrEmpty(actMaterial)) { actMaterial = m_groundMaterial; }

                //Get VertexStructure object
                VertexStructure actVertexStructure = null;
                if (materialRelated.ContainsKey(actMaterial)) { actVertexStructure = materialRelated[actMaterial]; }
                else
                {
                    actVertexStructure = new VertexStructure();
                    actVertexStructure.Material = actMaterial;
                    materialRelated[actMaterial] = actVertexStructure;
                    result.Add(actVertexStructure);
                }

                //Get position of the tile
                Vector3 tilePosition = new Vector3(
                    (actTile.XPos * m_tileSize.X) - totalHalfSize.X,
                    0f,
                    (actTile.YPos * m_tileSize.Y) - totalHalfSize.Y);

                //Add tile information to current VertexStructures
                actVertexStructure.BuildCubeTop4V(
                    new Vector3(tilePosition.X, -m_height, tilePosition.Z),
                    new Vector3(m_tileSize.X, m_height, m_tileSize.Y),
                    Color4.White);
                bottomStructure.BuildCubeBottom4V(
                    new Vector3(tilePosition.X, -m_height, tilePosition.Z),
                    new Vector3(m_tileSize.X, m_height, m_tileSize.Y),
                    Color4.White);
            }

            //Build all borders
            VertexStructure borderStructure = null;
            if (materialRelated.ContainsKey(m_borderMaterial)) { borderStructure = materialRelated[m_borderMaterial]; }
            else
            {
                borderStructure = new VertexStructure();
                borderStructure.Material = m_borderMaterial;
                materialRelated[m_borderMaterial] = borderStructure;
                result.Add(borderStructure);
            }
            foreach (BorderInformation actBorder in m_borders)
            {
                if (m_borderSize <= 0f)
                {
                    Vector3 tilePosition = new Vector3(
                        (actBorder.TileXPos * m_tileSize.X) - totalHalfSize.X,
                        0f,
                        (actBorder.TileYPos * m_tileSize.Y) - totalHalfSize.Y);

                    //Build simple borders
                    switch (actBorder.Location)
                    {
                        case BorderLocation.Left:
                            borderStructure.BuildRect4V(
                                new Vector3(tilePosition.X, -m_height, tilePosition.Z),
                                new Vector3(tilePosition.X, 0f, tilePosition.Z),
                                new Vector3(tilePosition.X, 0f, tilePosition.Z + m_tileSize.Y),
                                new Vector3(tilePosition.X, -m_height, tilePosition.Z + m_tileSize.Y));
                            break;

                        case BorderLocation.Top:
                            borderStructure.BuildRect4V(
                                new Vector3(tilePosition.X, -m_height, tilePosition.Z + m_tileSize.Y),
                                new Vector3(tilePosition.X, 0f, tilePosition.Z + m_tileSize.Y),
                                new Vector3(tilePosition.X + m_tileSize.X, 0f, tilePosition.Z + m_tileSize.Y),
                                new Vector3(tilePosition.X + m_tileSize.X, -m_height, tilePosition.Z + m_tileSize.Y));
                            break;

                        case BorderLocation.Right:
                            borderStructure.BuildRect4V(
                                new Vector3(tilePosition.X + m_tileSize.X, -m_height, tilePosition.Z + m_tileSize.Y),
                                new Vector3(tilePosition.X + m_tileSize.X, 0f, tilePosition.Z + m_tileSize.Y),
                                new Vector3(tilePosition.X + m_tileSize.X, 0f, tilePosition.Z),
                                new Vector3(tilePosition.X + m_tileSize.X, -m_height, tilePosition.Z));
                            break;

                        case BorderLocation.Bottom:
                            borderStructure.BuildRect4V(
                                new Vector3(tilePosition.X + m_tileSize.X, -m_height, tilePosition.Z),
                                new Vector3(tilePosition.X + m_tileSize.X, 0f, tilePosition.Z),
                                new Vector3(tilePosition.X, 0f, tilePosition.Z),
                                new Vector3(tilePosition.X, -m_height, tilePosition.Z));
                            break;
                    }
                }
                else
                {
                    //Build complex borders
                }
            }

            //Return all generated VertexStructures
            return result.ToArray();
        }

        /// <summary>
        /// Creates a boolean map out of given tilemap.
        /// </summary>
        /// <param name="tileMap">The tilemap to convert.</param>
        private bool[,] CreateBooleanMap(GroundTileInfo[,] tileMap)
        {
            int tilesX = tileMap.GetLength(0);
            int tilesY = tileMap.GetLength(1);

            //Convert tilemap
            bool[,] result = new bool[tilesX, tilesY];
            for (int loopX = 0; loopX < tilesX; loopX++)
            {
                for (int loopY = 0; loopY < tilesY; loopY++)
                {
                    result[loopX, loopY] = tileMap[loopX, loopY] != null;
                }
            }

            return result;
        }

        /// <summary>
        /// Generates borders for the ground.
        /// </summary>
        /// <param name="tileMap">The tilemap.</param>
        /// <param name="tilesX">Tiles in x direction.</param>
        /// <param name="tilesY">Tiles in y direction.</param>
        private void GenerateBorders(bool[,] tileMap, int tilesX, int tilesY)
        {
            m_borders.Clear();
            for (int loopX = 0; loopX < tilesX; loopX++)
            {
                for (int loopY = 0; loopY < tilesY; loopY++)
                {
                    if (tileMap[loopX, loopY])
                    {
                        if ((loopY == 0) || (!tileMap[loopX, loopY - 1]))
                        {
                            m_borders.Add(new BorderInformation(loopX, loopY, BorderLocation.Bottom));
                        }
                        if ((loopX == 0) || (!tileMap[loopX - 1, loopY]))
                        {
                            m_borders.Add(new BorderInformation(loopX, loopY, BorderLocation.Left));
                        }
                        if ((loopY == tilesY - 1) || (!tileMap[loopX, loopY + 1]))
                        {
                            m_borders.Add(new BorderInformation(loopX, loopY, BorderLocation.Top));
                        }
                        if ((loopX == tilesX - 1) || (!tileMap[loopX + 1, loopY]))
                        {
                            m_borders.Add(new BorderInformation(loopX, loopY, BorderLocation.Right));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the border material
        /// </summary>
        public string BorderMaterial
        {
            get { return m_borderMaterial; }
            set
            {
                if (m_borderMaterial != value)
                {
                    m_borderMaterial = value;
                    //TODO: Trigger reloading
                }
            }
        }

        /// <summary>
        /// Gets or sets the ground material
        /// </summary>
        public string DefaultFloorMaterial
        {
            get { return m_groundMaterial; }
            set
            {
                if (m_groundMaterial != value)
                {
                    m_groundMaterial = value;
                    //TODO: Trigger reloading
                }
            }
        }

        /// <summary>
        /// Gets or sets material for sides.
        /// </summary>
        public string SideMaterial
        {
            get { return m_sideMaterial; }
            set
            {
                if (m_sideMaterial != value)
                {
                    m_sideMaterial = value;
                    //TODO: Trigger reloading
                }
            }
        }

        /// <summary>
        /// Gets or sets the material for bottom.
        /// </summary>
        public string BottomMaterial
        {
            get { return m_bottomMaterial; }
            set
            {
                if (m_bottomMaterial != value)
                {
                    m_bottomMaterial = value;
                    //TODO: Trigger reloading
                }
            }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        private class BorderInformation
        {
            public int TileXPos;
            public int TileYPos;
            public BorderLocation Location;

            public BorderInformation(int xPos, int yPos, BorderLocation location)
            {
                this.TileXPos = xPos;
                this.TileYPos = yPos;
                this.Location = location;
            }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        private enum BorderLocation
        {
            Left, 
            LeftTop, 
            LeftBottom,
            Top,
            Right,
            RightTop,
            RightBottom,
            Bottom
        }
    }
}
