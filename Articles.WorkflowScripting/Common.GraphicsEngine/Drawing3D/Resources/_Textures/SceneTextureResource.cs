using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using Common.GraphicsEngine.Core;

//Some namespace mappings
using D3D11 = SlimDX.Direct3D11;
using DXGI  = SlimDX.DXGI;

namespace Common.GraphicsEngine.Drawing3D.Resources
{
    public class SceneTextureResource : TextureResource, IRenderableResource
    {
        //Resources for Direct3D 11 rendering
        private D3D11.ShaderResourceView m_renderTargetResourceView;
        private D3D11.Texture2D m_textureRenderTarget;
        private D3D11.Texture2D m_textureDepthBuffer;
        private D3D11.RenderTargetView m_renderTargetView;
        private D3D11.DepthStencilView m_depthStencilView;
        private D3D11.Viewport m_viewPort;

        //Generic members
        private Color4 m_backColor;
        private Scene m_scene;
        private Camera m_camera;
        private int m_textureWidth;
        private int m_textureHeight;

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneTextureResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        public SceneTextureResource(string name, int textureWidth, int textureHeight)
            : base(name)
        {
            m_camera = new Camera(textureWidth, textureHeight);
            m_scene = new Scene(m_camera);
            m_textureWidth = textureWidth;
            m_textureHeight = textureHeight;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneTextureResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        /// <param name="scene">The scene to be displayed.</param>
        /// <param name="camera">The camera for display.</param>
        public SceneTextureResource(string name, Scene scene, Camera camera)
            : base(name)
        {
            m_scene = scene;
            m_camera = camera;
            m_textureWidth = camera.ScreenWidth;
            m_textureHeight = camera.ScreenHeight;
        }

        /// <summary>
        /// Triggers internal update within the resource (e. g. Render to Texture).
        /// </summary>
        /// <param name="updateState">Current state of update process.</param>
        public void Update(UpdateState updateState)
        {
            //TODO: also push update state
            m_scene.Update(updateState);
        }

        /// <summary>
        /// Renders the scene to the texture.
        /// </summary>
        /// <param name="renderState">RenderState for rendering.</param>
        public void Render(RenderState renderState)
        {
            renderState.PushRenderTarget(
                m_renderTargetView,
                m_depthStencilView,
                m_viewPort, m_camera.ViewProjection);
            try
            {
                renderState.DeviceContext.ClearRenderTargetView(m_renderTargetView, m_backColor.ToDirectXColor());
                renderState.DeviceContext.ClearDepthStencilView(m_depthStencilView, D3D11.DepthStencilClearFlags.Depth, 1f, 0);

                m_scene.Render(renderState);
            }
            finally
            {
                renderState.PopRenderTarget();
            }
        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void LoadResourceInternal(ResourceDictionary resources)
        {
            if (m_textureRenderTarget == null)
            {
                //Get the device
                D3D11.Device device = GraphicsCore.Current.HandlerD3D11.Device;

                try
                {
                    //Create the texture
                    m_textureRenderTarget = GraphicsHelper.CreateRenderTargetTexture(device, m_textureWidth, m_textureHeight);
                    m_textureDepthBuffer = GraphicsHelper.CreateDepthBufferTexture(device, m_textureWidth, m_textureHeight);

                    //Create views needed for rendering
                    m_renderTargetView = new D3D11.RenderTargetView(device, m_textureRenderTarget);
                    m_depthStencilView = new D3D11.DepthStencilView(device, m_textureDepthBuffer);
                    m_renderTargetResourceView = new D3D11.ShaderResourceView(device, m_textureRenderTarget);
                }
                catch(Exception ex)
                {
                    //Unlaod all loaded resources in case of an error
                    UnloadResource(resources);
                    throw ex;
                }

                //Create the viewport
                m_viewPort = GraphicsHelper.CreateDefaultViewport(m_textureWidth, m_textureHeight);
            }
        }

        /// <summary>
        /// Unloads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void UnloadResourceInternal(ResourceDictionary resources)
        {
            if (m_textureRenderTarget != null)
            {
                m_scene.UnloadResources();

                m_depthStencilView = GraphicsHelper.DisposeGraphicsObject(m_depthStencilView);
                m_renderTargetView = GraphicsHelper.DisposeGraphicsObject(m_renderTargetView);
                m_renderTargetResourceView = GraphicsHelper.DisposeGraphicsObject(m_renderTargetResourceView);
                m_textureDepthBuffer = GraphicsHelper.DisposeGraphicsObject(m_textureDepthBuffer);
                m_textureRenderTarget = GraphicsHelper.DisposeGraphicsObject(m_textureRenderTarget);
            }
        }

        /// <summary>
        /// Is the resource loaded?
        /// </summary>
        public override bool IsLoaded
        {
            get { return m_textureRenderTarget != null; }
        }

        /// <summary>
        /// Gets the texture.
        /// </summary>
        public override D3D11.Texture2D Texture
        {
            get { return m_textureRenderTarget; }
        }

        /// <summary>
        /// Gets a ShaderResourceView targeting the texture.
        /// </summary>
        public override D3D11.ShaderResourceView TextureView
        {
            get { return m_renderTargetResourceView; }
        }

        /// <summary>
        /// Gets the scene.
        /// </summary>
        public Scene Scene
        {
            get { return m_scene; }
        }

        /// <summary>
        /// Gets the camera.
        /// </summary>
        public Camera Camera
        {
            get { return m_camera; }
        }

        /// <summary>
        /// Gets or sets the backcolor.
        /// </summary>
        public Color4 BackColor
        {
            get { return m_backColor; }
            set { m_backColor = value; }
        }
    }
}
