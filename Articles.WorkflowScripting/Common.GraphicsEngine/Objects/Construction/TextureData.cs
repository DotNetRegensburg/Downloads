using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using SlimDX;

namespace Common.GraphicsEngine.Objects.Construction
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TextureData
    {
        private Vector2 m_coordiante1;
        private Vector2 m_coordinate2;

        /// <summary>
        /// Initializes a new TextureData structure
        /// </summary>
        public TextureData(Vector2 coord1)
        {
            m_coordiante1 = coord1;
            m_coordinate2 = coord1;
        }

        /// <summary>
        /// Copies this structure and changes some data
        /// </summary>
        public TextureData Copy(Vector2 newCoord1)
        {
            TextureData result = this;
            result.m_coordiante1 = newCoord1;
            return result;
        }

        /// <summary>
        /// Retrieves or sets first texture coordinate
        /// </summary>
        public Vector2 Coordinate1
        {
            get { return m_coordiante1; }
            set { m_coordiante1 = value; }
        }

        /// <summary>
        /// Gets or sets the second texture coordinate.
        /// </summary>
        public Vector2 Coordinate2
        {
            get { return m_coordinate2; }
            set { m_coordinate2 = value; }
        }
    }
}
