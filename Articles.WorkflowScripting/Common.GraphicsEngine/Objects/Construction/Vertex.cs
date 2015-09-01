using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using SlimDX;

namespace Common.GraphicsEngine.Objects.Construction
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        private GeometryData m_geoData;
        private TextureData m_textureData;
        private AnimationData m_animationData;

        /// <summary>
        /// Creates a new vertex
        /// </summary>
        public Vertex(Vector3 position)
        {
            m_geoData = new GeometryData(position);
            m_textureData = new TextureData();
            m_animationData = new AnimationData();
        }

        /// <summary>
        /// Creates a new vertex
        /// </summary>
        public Vertex(Vector3 position, Color4 color)
        {
            m_geoData = new GeometryData(position, color);
            m_textureData = new TextureData();
            m_animationData = new AnimationData();
        }

        /// <summary>
        /// Creates a new vertex
        /// </summary>
        public Vertex(Vector3 position, Color4 color, Vector2 texCoord1)
        {
            m_geoData = new GeometryData(position, color);
            m_textureData = new TextureData(texCoord1);
            m_animationData = new AnimationData();
        }

        /// <summary>
        /// Creates a new vertex
        /// </summary>
        public Vertex(Vector3 position, Color4 color, Vector2 texCoord1, Vector3 normal)
        {
            m_geoData = new GeometryData(position, normal, color);
            m_textureData = new TextureData(texCoord1);
            m_animationData = new AnimationData();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return m_geoData.Position.ToString();
        }

        /// <summary>
        /// Copies this vertex and sets the new position
        /// </summary>
        public Vertex Copy(Vector3 newPosition)
        {
            Vertex result = this;
            result.m_geoData = result.m_geoData.Copy(newPosition);
            return result;
        }

        /// <summary>
        /// Copies this vertex and sets a new position and texture coordinate
        /// </summary>
        public Vertex Copy(Vector3 newPosition, Vector2 newTexCoord1)
        {
            Vertex result = this;
            result.m_geoData = result.m_geoData.Copy(newPosition);
            result.m_textureData = result.m_textureData.Copy(newTexCoord1);
            return result;
        }

        /// <summary>
        /// Copies this vertex and sets a new position, normal and texture coordinate
        /// </summary>
        public Vertex Copy(Vector3 newPosition, Vector3 newNormal, Vector2 newTexCoord1)
        {
            Vertex result = this;
            result.m_geoData = result.m_geoData.Copy(newPosition, newNormal);
            result.m_textureData = result.m_textureData.Copy(newTexCoord1);
            return result;
        }

        /// <summary>
        /// Copies this vertex and sets the new values
        /// </summary>
        public Vertex Copy(Vector3 newPosition, Vector3 newNormal)
        {
            Vertex result = this;
            result.m_geoData = result.m_geoData.Copy(newPosition, newNormal);
            return result;
        }

        /// <summary>
        /// Retrieves or sets geometry data
        /// </summary>
        public GeometryData Geometry
        {
            get { return m_geoData; }
            set { m_geoData = value; }
        }

        /// <summary>
        /// Gets or sets all animation related data of the vertex.
        /// </summary>
        public AnimationData Animation
        {
            get { return m_animationData; }
            set { m_animationData = value; }
        }

        /// <summary>
        /// Retrieves or sets texture data
        /// </summary>
        public TextureData Texture
        {
            get { return m_textureData; }
            set { m_textureData = value; }
        }

        /// <summary>
        /// Gets or sets the position
        /// </summary>
        public Vector3 Position
        {
            get { return m_geoData.Position; }
            set { m_geoData.Position = value; }
        }

        /// <summary>
        /// Gets or sets the normal
        /// </summary>
        public Vector3 Normal
        {
            get { return m_geoData.Normal; }
            set { m_geoData.Normal = value; }
        }

        /// <summary>
        /// Gets or sets the tangent vector.
        /// </summary>
        public Vector3 Tangent
        {
            get { return m_geoData.Tangent; }
            set { m_geoData.Tangent = value; }
        }

        /// <summary>
        /// Gets or sets the binormal vector.
        /// </summary>
        public Vector3 Binormal
        {
            get { return m_geoData.Binormal; }
            set { m_geoData.Binormal = value; }
        }

        /// <summary>
        /// Gets or sets the texture coordinate
        /// </summary>
        public Vector2 TexCoord
        {
            get { return m_textureData.Coordinate1; }
            set { m_textureData.Coordinate1 = value; }
        }

        /// <summary>
        /// Gets or sets the diffuse color
        /// </summary>
        public Color4 Color
        {
            get { return m_geoData.Color; }
            set { m_geoData.Color = value; }
        }
    }
}
