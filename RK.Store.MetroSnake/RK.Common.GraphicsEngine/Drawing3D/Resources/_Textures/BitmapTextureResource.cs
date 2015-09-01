using System;
using System.Drawing;
using System.IO;
using RK.Common.GraphicsEngine.Core;
using RK.Common.Util;

//Some namespace mappings
using D3D11 = SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;

namespace RK.Common.GraphicsEngine.Drawing3D.Resources
{
    public class BitmapTextureResource : TextureResource
    {
        //Member for Direct3D 11 rendering
        private D3D11.Texture2D m_texture;
        private D3D11.ShaderResourceView m_textureView;

        //Generic members
        private AssemblyResourceLink m_resourceLink;
        private string m_sourceFile;
        private Bitmap m_bitmap;

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapTextureResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        /// <param name="sourceFile">Path to the texture file.</param>
        public BitmapTextureResource(string name, string sourceFile)
            : base(name)
        {
            m_sourceFile = sourceFile;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapTextureResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        /// <param name="bitmap">The bitmap source object.</param>
        public BitmapTextureResource(string name, Bitmap bitmap)
            : base(name)
        {
            m_bitmap = bitmap;
        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void LoadResourceInternal(ResourceDictionary resources)
        {
            if (m_texture == null)
            {
                //Get the device
                D3D11.Device device = GraphicsCore.Current.HandlerD3D11.Device;

                //Get source bitmap
                Bitmap bitmap = m_bitmap;

                if (m_bitmap != null)
                {
                    m_texture = GraphicsHelper.CreateTextureFromBitmap(device, bitmap, 0);
                }
                else if (!string.IsNullOrEmpty(m_sourceFile))
                {
                    if (string.IsNullOrEmpty(m_sourceFile)) { throw new ArgumentException("No texture source set!"); }
                    if (!File.Exists(m_sourceFile)) { throw new ArgumentException("Texture source file " + m_sourceFile + " does not exist!"); }
    
                    //Load texture form file
                    m_texture = D3D11.Texture2D.FromFile(device, m_sourceFile, new D3D11.ImageLoadInformation()
                    {
                        BindFlags = D3D11.BindFlags.ShaderResource,
                        CpuAccessFlags = D3D11.CpuAccessFlags.None,
                        Filter = D3D11.FilterFlags.None,
                        Format = DXGI.Format.R8G8B8A8_UNorm,
                        MipLevels = 0,
                        Usage = D3D11.ResourceUsage.Immutable,
                        OptionFlags = D3D11.ResourceOptionFlags.None
                    }) as D3D11.Texture2D;
                }
                else if (m_resourceLink != null)
                {
                    //Load texture form a resource file
                    Stream inStream = m_resourceLink.OpenRead();
                    try
                    {
                        m_texture = D3D11.Texture2D.FromStream(device, inStream, (int)inStream.Length, new D3D11.ImageLoadInformation()
                        {
                            BindFlags = D3D11.BindFlags.ShaderResource,
                            CpuAccessFlags = D3D11.CpuAccessFlags.None,
                            Filter = D3D11.FilterFlags.None,
                            Format = DXGI.Format.R8G8B8A8_UNorm,
                            MipLevels = 0,
                            Usage = D3D11.ResourceUsage.Immutable,
                            OptionFlags = D3D11.ResourceOptionFlags.None
                        }) as D3D11.Texture2D;
                    }
                    finally
                    {
                        if (inStream != null) { inStream.Close(); }
                    }
                }

                //Create the view targeting the texture
                m_textureView = new D3D11.ShaderResourceView(device, m_texture);
            }
        }

        /// <summary>
        /// Unloads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void UnloadResourceInternal(ResourceDictionary resources)
        {
            if (m_texture != null)
            {
                m_textureView = GraphicsHelper.DisposeGraphicsObject(m_textureView);
                m_texture = GraphicsHelper.DisposeGraphicsObject(m_texture);
            }
        }

        /// <summary>
        /// Sets the bitmap to be displayed.
        /// </summary>
        protected void SetBitmap(Bitmap bitmap)
        {
            if (bitmap == null) { throw new ArgumentNullException("bitmap"); }

            m_bitmap = bitmap;
            m_sourceFile = string.Empty;

            base.ReloadResource();
        }

        /// <summary>
        /// Changes the source file.
        /// </summary>
        protected void SetSourceFile(string sourceFile)
        {
            if (sourceFile == null) { throw new ArgumentNullException("sourceFile"); }

            m_bitmap = null;
            m_sourceFile = sourceFile;

            base.ReloadResource();
        }

        /// <summary>
        /// Is the resource loaded?
        /// </summary>
        public override bool IsLoaded
        {
            get { return m_texture != null; }
        }

        /// <summary>
        /// Gets the texture.
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
