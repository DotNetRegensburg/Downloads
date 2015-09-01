using RK.Common.GraphicsEngine.Core;

//Some namespace mappings
using D3D11 = SharpDX.Direct3D11;

namespace RK.Common.GraphicsEngine.Drawing3D.Resources
{
    public class CopiedTextureResource : TextureResource
    {
        //Direct3D 11 Resources
        private D3D11.Texture2D m_texture;
        private D3D11.ShaderResourceView m_textureView;

        //Standard members
        private ICopiedTextureSource m_source;
        private int m_currentWidth;
        private int m_currentHeight;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopiedTextureResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        public CopiedTextureResource(string name, ICopiedTextureSource source)
            : base(name)
        {
            m_source = source;
        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void LoadResourceInternal(ResourceDictionary resources)
        {
            if ((m_source.TextureWidth <= 0) || (m_source.TextureHeight <= 0) || (m_source.Texture == null)) { return; }
            
            UpdateTexture();

            m_source.TextureChanged += OnSourceTextureChanged;
        }

        /// <summary>
        /// Unloads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void UnloadResourceInternal(ResourceDictionary resources)
        {
            m_textureView = GraphicsHelper.DisposeGraphicsObject(m_textureView);
            m_texture = GraphicsHelper.DisposeGraphicsObject(m_texture);

            m_currentWidth = 0;
            m_currentHeight = 0;

            m_source.TextureChanged -= OnSourceTextureChanged;
        }

        /// <summary>
        /// Updates the texture (handles resizing of the source texture).
        /// </summary>
        private void UpdateTexture()
        {
            D3D11.Device device = GraphicsCore.Current.HandlerD3D11.Device;

            if ((m_currentWidth != m_source.TextureWidth) || (m_currentHeight != m_source.TextureHeight))
            {
                m_textureView = GraphicsHelper.DisposeGraphicsObject(m_textureView);
                m_texture = GraphicsHelper.DisposeGraphicsObject(m_texture);

                m_texture = GraphicsHelper.CreateRenderTargetTexture(device, m_source.TextureWidth, m_source.TextureHeight);
                m_textureView = new D3D11.ShaderResourceView(device, m_texture);

                m_currentWidth = m_source.TextureWidth;
                m_currentHeight = m_source.TextureHeight;
            }
        }

        /// <summary>
        /// Called when source texture has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Testing.DirectX11Engine.Graphics.Resources.TextureChangedEventArgs"/> instance containing the event data.</param>
        private void OnSourceTextureChanged(object sender, TextureChangedEventArgs e)
        {
            if ((m_source.TextureWidth <= 0) || (m_source.TextureHeight <= 0) || (m_source.Texture == null)) { return; }

            UpdateTexture();

            //Copy contents of source texture into current texture
            e.RenderState.DeviceContext.CopyResource(m_source.Texture, m_texture);
        }

        /// <summary>
        /// Is the resource loaded?
        /// </summary>
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
