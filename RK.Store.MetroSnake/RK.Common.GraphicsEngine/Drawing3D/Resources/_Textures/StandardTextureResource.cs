using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Threading.Tasks;
using RK.Common.GraphicsEngine.Core;
using RK.Common.Util;

//Some namespace mappings
using WIC = SharpDX.WIC;
using D3D11 = SharpDX.Direct3D11;

namespace RK.Common.GraphicsEngine.Drawing3D.Resources
{
    public class StandardTextureResource : TextureResource
    {
        //Given configuration
        private string m_fileName;
        private AssemblyResourceLink m_resourceLink;
        private Uri m_textureResourceUri;

        //Loaded resources
        private D3D11.Texture2D m_texture;
        private D3D11.ShaderResourceView m_textureView;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardTextureResource" /> class.
        /// </summary>
        /// <param name="name">The name of the generated resource.</param>
        /// <param name="fileName">Name of the file.</param>
        public StandardTextureResource(string name, string fileName)
            : base(name)
        {
            m_fileName = fileName;
            m_resourceLink = null;
            m_textureResourceUri = null;
        }

#if DESKTOP
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardTextureResource" /> class.
        /// </summary>
        /// <param name="name">The name of the generated resource.</param>
        /// <param name="resourceUri">The resource uri.</param>
        public StandardTextureResource(string name, Uri resourceUri)
            : base(name)
        {
            m_fileName = string.Empty;
            m_textureResourceUri = resourceUri;
            m_resourceLink = null;
        }
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardTextureResource" /> class.
        /// </summary>
        /// <param name="name">The name of the generated resource.</param>
        /// <param name="resourceLink">The resource link.</param>
        public StandardTextureResource(string name, AssemblyResourceLink resourceLink)
            : base(name)
        {
            m_fileName = string.Empty;
            m_resourceLink = resourceLink;
            m_resourceLink = null;
        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        /// <exception cref="GraphicsEngineException"></exception>
        protected override void LoadResourceInternal(ResourceDictionary resources)
        {
            WIC.ImagingFactory wicFactory = GraphicsCore.Current.HandlerWIC.Factory;
            D3D11.Device device = GraphicsCore.Current.HandlerD3D11.Device;

            //Load the texture if possible
            if (!string.IsNullOrEmpty(m_fileName)) { m_texture = GraphicsHelper.LoadTexture2D(wicFactory, device, m_fileName); }
#if DESKTOP
            else if (m_textureResourceUri != null)
            {
                using (Stream inStream = Application.GetResourceStream(m_textureResourceUri).Stream)
                {
                    m_texture = GraphicsHelper.LoadTexture2D(wicFactory, device, inStream);
                }
            }
#endif
            else if (m_resourceLink != null)
            {
                using (Stream inStream = m_resourceLink.OpenRead())
                {
                    m_texture = GraphicsHelper.LoadTexture2D(wicFactory, device, inStream);
                }
            }
            else { throw new GraphicsEngineException("No texture source found for resource + " + this.Name + "!"); }

            //Create view for shaders
            m_textureView = new D3D11.ShaderResourceView(device, m_texture);
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
        /// Gets the texture object.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
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

        /// <summary>
        /// Is the object loaded correctly?
        /// </summary>
        public override bool IsLoaded
        {
            get { return m_textureView != null; }
        }
    }
}
