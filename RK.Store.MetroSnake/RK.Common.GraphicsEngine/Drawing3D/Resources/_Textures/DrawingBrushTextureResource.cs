using System.Drawing;
using RK.Common.GraphicsEngine.Core;
using SharpDX;

//Some namespace mappings
using D3D11 = SharpDX.Direct3D11;
using GDI = System.Drawing;

namespace RK.Common.GraphicsEngine.Drawing3D.Resources
{
    public class DrawingBrushTextureResource : TextureResource
    {
        //Direct3D 11 Resources
        private D3D11.Texture2D m_texture;
        private D3D11.ShaderResourceView m_textureView;

        //Standard members
        private Brush m_drawingBrush;
        private int m_width;
        private int m_height;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingBrushTextureResource"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="drawingBrush">The drawing brush.</param>
        public DrawingBrushTextureResource(string name, Brush drawingBrush)
            : this(name, drawingBrush, 128, 128)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingBrushTextureResource"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="drawingBrush">The drawing brush.</param>
        /// <param name="height">Width of the texture.</param>
        /// <param name="width">Height of the texture.</param>
        public DrawingBrushTextureResource(string name, Brush drawingBrush, int width, int height)
            : base(name)
        {
            m_drawingBrush = drawingBrush;
            m_width = width;
            m_height = height;
        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void LoadResourceInternal(ResourceDictionary resources)
        {
            D3D11.Device device = GraphicsCore.Current.HandlerD3D11.Device;

            using(Bitmap drawingBitmap = new Bitmap(m_width, m_height))
            {
                using (GDI.Graphics graphics = GDI.Graphics.FromImage(drawingBitmap))
                {
                    graphics.FillRectangle(
                        m_drawingBrush,
                        new GDI.Rectangle(0, 0, m_width, m_height));
                    graphics.Dispose();
                }

                m_texture = GraphicsHelper.CreateTextureFromBitmap(device, drawingBitmap);
                m_textureView = new D3D11.ShaderResourceView(device, m_texture);
            }
        }

        /// <summary>
        /// Unloads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void UnloadResourceInternal(ResourceDictionary resources)
        {
            m_textureView = GraphicsHelper.DisposeGraphicsObject(m_textureView);
            m_texture = GraphicsHelper.DisposeGraphicsObject(m_texture);
        }

        /// <summary>
        /// Is the resource loaded?
        /// </summary>
        /// <value></value>
        public override bool IsLoaded
        {
            get { return (m_texture != null) && (m_textureView != null); }
        }

        /// <summary>
        /// Gets the texture object.
        /// </summary>
        public override D3D11.Texture2D Texture
        {
            get { return m_texture; }
        }

        /// <summary>
        /// Gets a ShaderResourceView targeting the texture.
        /// </summary>
        public override D3D11.ShaderResourceView TextureView
        {
            get { return m_textureView; }
        }
    }
}
