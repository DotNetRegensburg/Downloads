using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RK.Common.GraphicsEngine.Objects
{
    public class GroundTileInfo
    {
        private string m_material;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroundTileInfo"/> class.
        /// </summary>
        /// <param name="material">The material to use for the tile (string.Empty or null to use default material).</param>
        public GroundTileInfo(string material)
        {
            m_material = material;
        }

        /// <summary>
        /// Gets the material used for this tile.
        /// </summary>
        public string Material
        {
            get { return m_material; }
        }
    }
}
