using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RK.Common.GraphicsEngine.Objects
{
    public class GroundTile
    {
        private int m_xPos;
        private int m_yPos;
        private GroundTileInfo m_tileInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroundTile"/> class.
        /// </summary>
        /// <param name="xPos">The x pos.</param>
        /// <param name="yPos">The y pos.</param>
        internal GroundTile(int xPos, int yPos)
        {
            m_xPos = xPos;
            m_yPos = yPos;
            m_tileInfo = new GroundTileInfo(string.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroundTile"/> class.
        /// </summary>
        /// <param name="xPos">The x pos.</param>
        /// <param name="yPos">The y pos.</param>
        /// <param name="tileInfo">Gets some generic information about the tile.</param>
        internal GroundTile(int xPos, int yPos, GroundTileInfo tileInfo)
        {
            m_xPos = xPos;
            m_yPos = yPos;
            m_tileInfo = tileInfo;
        }

        /// <summary>
        /// Gets the material used by this tile.
        /// </summary>
        public string Material
        {
            get { return m_tileInfo.Material; }
        }

        /// <summary>
        /// Gets the x-position of the tile.
        /// </summary>
        public int XPos
        {
            get { return m_xPos; }
        }

        /// <summary>
        /// Gets the y-position of the tile.
        /// </summary>
        public int YPos
        {
            get { return m_yPos; }
        }
    }
}
