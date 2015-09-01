using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using SlimDX;

namespace Common.GraphicsEngine.Objects.Construction
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GeometryData
    {
        private Vector3 m_position;
        private Vector3 m_normal;
        private Vector3 m_tangent;
        private Vector3 m_binormal;
        private Color4 m_color;

        /// <summary>
        /// Initializes a new geometry data structure
        /// </summary>
        public GeometryData(Vector3 position)
        {
            m_position = position;
            m_normal = Vector3.Empty;
            m_tangent = Vector3.Empty;
            m_color = Color4.White;
            m_tangent = Vector3.Empty;
            m_binormal = Vector3.Empty;
        }

        /// <summary>
        /// Initializes a new geometry data structure
        /// </summary>
        public GeometryData(Vector3 position, Color4 color)
        {
            m_position = position;
            m_normal = Vector3.Empty;
            m_color = color;
            m_tangent = Vector3.Empty;
            m_binormal = Vector3.Empty;
        }

        /// <summary>
        /// Initializes a new geometry data structure
        /// </summary>
        public GeometryData(Vector3 position, Vector3 normal, Color4 color)
        {
            m_position = position;
            m_normal = normal;
            m_color = color;
            m_tangent = Vector3.Empty;
            m_binormal = Vector3.Empty;
        }

        /// <summary>
        /// Initializes a new geometry data structure
        /// </summary>
        public GeometryData(Vector3 position, Vector3 normal)
        {
            m_position = position;
            m_normal = normal;
            m_color = Color4.White;
            m_tangent = Vector3.Empty;
            m_binormal = Vector3.Empty;
        }

        /// <summary>
        /// Copies this structure and replaces the given values
        /// </summary>
        public GeometryData Copy(Vector3 newPosition)
        {
            GeometryData result = this;
            result.m_position = newPosition;
            return result;
        }

        /// <summary>
        /// Copies this structure and replaces the given values
        /// </summary>
        public GeometryData Copy(Vector3 newPosition, Vector3 newNormal)
        {
            GeometryData result = this;
            result.m_position = newPosition;
            result.m_normal = newNormal;
            return result;
        }

        /// <summary>
        /// Retrieves or sets the position of the vertex
        /// </summary>
        public Vector3 Position
        {
            get { return m_position; }
            set { m_position = value; }
        }

        /// <summary>
        /// Retrieves or sets the normal of the vertex
        /// </summary>
        public Vector3 Normal
        {
            get { return m_normal; }
            set { m_normal = value; }
        }

        /// <summary>
        /// Retrieves or sets the color of the vertex
        /// </summary>
        public Color4 Color
        {
            get { return m_color; }
            set { m_color = value; }
        }

        /// <summary>
        /// Gets or sets the tangent vector.
        /// </summary>
        public Vector3 Tangent
        {
            get { return m_tangent; }
            set { m_tangent = value; }
        }

        /// <summary>
        /// Gets or sets the binormal vector.
        /// </summary>
        public Vector3 Binormal
        {
            get { return m_binormal; }
            set { m_binormal = value; }
        }
    }
}
