//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using SlimDX;

////Some namespace mappings
//using DXGI   = SlimDX.DXGI;
//using D3D10  = SlimDX.Direct3D10;
//using D3D101 = SlimDX.Direct3D10_1;
//using D3D11  = SlimDX.Direct3D11;
//using D2D    = SlimDX.Direct2D;
//using Vector3 = Common.Mathematics.Vector3;
//using Vector2 = Common.Mathematics.Vector2;
//using Color4 = Common.Mathematics.Color4;

//namespace Common.GraphicsEngine.Drawing3D.Resources
//{
//    public class Scene2DTextureResource : TextureResource, IRenderableResource
//    {
//        //DirectX 11 Resources
//        private D3D11.Texture2D m_texture;
//        private D3D11.ShaderResourceView m_textureView;

//        //DirectX 10 Resources
//        private D3D10.Texture2D m_sharingTexture;

//        //Direct2D Resources
//        private D2D.RenderTarget m_renderTarget;

//        //DXGI Resources
//        private IntPtr m_sharedHandle;
//        private DXGI.Resource m_resource;
//        private DXGI.Surface m_surface;

//        //Standard members
//        private Color4 m_background;
//        private RenderState2D m_renderState;
//        private Scene2D m_scene;
//        private Camera2D m_camera;
//        private int m_textureWidth;
//        private int m_textureHeight;
//        private bool m_performFallBack;

//        [Category("Renderer")]
//        public event Rendering2DHandler BeforeRendering;

//        [Category("Renderer")]
//        public event Rendering2DHandler AfterRendering;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="Scene2DTextureResource"/> class.
//        /// </summary>
//        /// <param name="name">The name of the resource.</param>
//        public Scene2DTextureResource(string name, int textureWidth, int textureHeight)
//            : base(name)
//        {
//            m_textureWidth = textureWidth;
//            m_textureHeight = textureHeight;

//            m_scene = new Scene2D();
//            m_camera = new Camera2D();

//            m_background = Color4.LightSteelBlue;
//        }

//        /// <summary>
//        /// Triggers internal update within the resource (e. g. Render to Texture).
//        /// </summary>
//        /// <param name="updateState">Current state of update process.</param>
//        public void Update(UpdateState updateState)
//        {
            
//        }

//        /// <summary>
//        /// Renders the texture.
//        /// </summary>
//        public void Render(RenderState renderState)
//        {
//            if (m_performFallBack)
//            {
//                m_scene.PrepareRendering(m_renderState);

//                m_renderTarget.BeginDraw();
//                {
//                    //Raise BeforeRendering event.
//                    RaiseBeforeRendering();

//                    m_renderTarget.Clear(m_background.ToDirectXColor());
//                    m_scene.Render(m_renderState);

//                    //Raise AfterRendering event.
//                    RaiseAfterRendering();

//                    //Executes all pending drawing calls
//                    m_renderTarget.Flush();
//                }
//                m_renderTarget.EndDraw();

//                //... In case of changes, copy dxgi resources
//            }
//            else
//            {
//                m_scene.PrepareRendering(m_renderState);

//                m_renderTarget.BeginDraw();
//                {
//                    //Raise BeforeRendering event.
//                    RaiseBeforeRendering();

//                    m_renderTarget.Clear(m_background.ToDirectXColor());
//                    m_scene.Render(m_renderState);

//                    //Raise AfterRendering event.
//                    RaiseAfterRendering();

//                    //Executes all pending drawing calls
//                    m_renderTarget.Flush();
//                }
//                m_renderTarget.EndDraw();

//                //Workaround: Change this in future (Content is rendered to direct3D surface after a fiew EndDraw calls
//                for (int loop = 0; loop < 20; loop++)
//                {
//                    m_renderTarget.BeginDraw();
//                    m_renderTarget.EndDraw();

//                }
//            }
//        }

//        /// <summary>
//        /// Loads the resource.
//        /// </summary>
//        /// <param name="resources">Parent ResourceDictionary.</param>
//        protected override void LoadResourceInternal(ResourceDictionary resources)
//        {
//            D3D11.Device deviceD3D11 = GraphicsCore.Current.HandlerD3D11.Device;
//            D3D101.Device1 deviceD3D10 = GraphicsCore.Current.HandlerD3D101.Device;

//            if (!GraphicsCore.Current.HandlerD3D11.IsDirect2DTextureEnabled)
//            {
//                //Create Direct3D 10 render target
//                m_sharingTexture = GraphicsHelper.CreateSharingTexture(deviceD3D10, m_textureWidth, m_textureHeight); //GraphicsHelper.CreateRenderTargetTexture10(deviceD3D10, m_textureWidth, m_textureHeight);
//                m_surface = m_sharingTexture.AsSurface();

//                //Create Direct2D Render Target
//                m_renderTarget = D2D.RenderTarget.FromDXGI(
//                    GraphicsCore.Current.HandlerD2D.Factory,
//                    m_surface,
//                    new D2D.RenderTargetProperties()
//                    {
//                        Type = GraphicsCore.Current.TargetHardware == TargetHardware.SoftwareRenderer ? D2D.RenderTargetType.Software : D2D.RenderTargetType.Hardware,
//                        Usage = D2D.RenderTargetUsage.ForceBitmapRemoting,
//                        PixelFormat = new D2D.PixelFormat(DXGI.Format.B8G8R8A8_UNorm, D2D.AlphaMode.Premultiplied),
//                        MinimumFeatureLevel = D2D.FeatureLevel.Direct3D10,
//                        HorizontalDpi = GraphicsCore.Current.HandlerD2D.Factory.DesktopDpi.Width,
//                        VerticalDpi = GraphicsCore.Current.HandlerD2D.Factory.DesktopDpi.Height
//                    });

//                //Create Direct3D 11 texture
//                m_texture = GraphicsHelper.CreateTexture(deviceD3D11, m_textureWidth, m_textureHeight);
//                m_textureView = new D3D11.ShaderResourceView(deviceD3D11, m_texture);

//                //Creates the RenderState2D object
//                m_renderState = new RenderState2D(m_renderTarget);

//                //Enable fallback mode 
//                m_performFallBack = true;
//            }
//            else
//            {
//                m_sharingTexture = GraphicsHelper.CreateSharingTexture(deviceD3D10, m_textureWidth, m_textureHeight);

//                //Get DirectX 11 Reference to the texture
//                m_surface = m_sharingTexture.AsSurface();
//                m_resource = new DXGI.Resource(m_surface);
//                m_sharedHandle = m_resource.SharedHandle;
//                m_texture = deviceD3D11.OpenSharedResource<D3D11.Texture2D>(m_sharedHandle);
//                m_textureView = new D3D11.ShaderResourceView(deviceD3D11, m_texture);

//                //Create Direct2D Render Target
//                m_renderTarget = D2D.RenderTarget.FromDXGI(
//                    GraphicsCore.Current.HandlerD2D.Factory,
//                    m_surface,
//                    new D2D.RenderTargetProperties()
//                {
//                    Type = GraphicsCore.Current.TargetHardware == TargetHardware.SoftwareRenderer ? D2D.RenderTargetType.Software : D2D.RenderTargetType.Hardware,
//                    Usage = D2D.RenderTargetUsage.ForceBitmapRemoting,
//                    PixelFormat = new D2D.PixelFormat(DXGI.Format.B8G8R8A8_UNorm, D2D.AlphaMode.Premultiplied),
//                    MinimumFeatureLevel = D2D.FeatureLevel.Direct3D10,
//                    HorizontalDpi = GraphicsCore.Current.HandlerD2D.Factory.DesktopDpi.Width,
//                    VerticalDpi = GraphicsCore.Current.HandlerD2D.Factory.DesktopDpi.Height
//                });

//                //Creates the RenderState2D object
//                m_renderState = new RenderState2D(m_renderTarget);

//                //Disable fallback mode
//                m_performFallBack = false;
//            }
//        }

//        /// <summary>
//        /// Unloads the resource.
//        /// </summary>
//        /// <param name="resources">Parent ResourceDictionary.</param>
//        protected override void UnloadResourceInternal(ResourceDictionary resources)
//        {
//            if (m_performFallBack)
//            {
//                m_scene.UnloadResources();

//                m_textureView = GraphicsHelper.DisposeGraphicsObject(m_textureView);
//                m_texture = GraphicsHelper.DisposeGraphicsObject(m_texture);
//                m_renderTarget = GraphicsHelper.DisposeGraphicsObject(m_renderTarget);
//                m_surface = GraphicsHelper.DisposeGraphicsObject(m_surface);
//                m_sharingTexture = GraphicsHelper.DisposeGraphicsObject(m_sharingTexture);
//            }
//            else
//            {
//                m_scene.UnloadResources();

//                m_renderTarget = GraphicsHelper.DisposeGraphicsObject(m_renderTarget);
//                m_textureView = GraphicsHelper.DisposeGraphicsObject(m_textureView);
//                m_texture = GraphicsHelper.DisposeGraphicsObject(m_texture);
//                m_resource = GraphicsHelper.DisposeGraphicsObject(m_resource);
//                m_surface = GraphicsHelper.DisposeGraphicsObject(m_surface);
//                m_sharingTexture = GraphicsHelper.DisposeGraphicsObject(m_sharingTexture);
//            }

//            //Disable fallback mode
//            m_performFallBack = false;
//        }

//        /// <summary>
//        /// Raises the BeforeRendering event.
//        /// </summary>
//        private void RaiseBeforeRendering()
//        {
//            if (BeforeRendering != null) { BeforeRendering(this, new Rendering2DArgs(m_renderState)); }
//        }

//        /// <summary>
//        /// Raises the AfterRendering event.
//        /// </summary>
//        private void RaiseAfterRendering()
//        {
//            if (AfterRendering != null) { AfterRendering(this, new Rendering2DArgs(m_renderState)); }
//        }

//        /// <summary>
//        /// Is the resource loaded?
//        /// </summary>
//        public override bool IsLoaded
//        {
//            get { return m_texture != null; }
//        }

//        /// <summary>
//        /// Gets the texture object.
//        /// </summary>
//        public override D3D11.Texture2D Texture
//        {
//            get { return m_texture; }
//        }

//        /// <summary>
//        /// Gets a ShaderResourceView targeting the texture.
//        /// </summary>
//        public override D3D11.ShaderResourceView TextureView
//        {
//            get { return m_textureView; }
//        }

//        /// <summary>
//        /// Gets the scene.
//        /// </summary>
//        public Scene2D Scene
//        {
//            get { return m_scene; }
//        }

//        /// <summary>
//        /// Gets the camera.
//        /// </summary>
//        public Camera2D Camera
//        {
//            get { return m_camera; }
//        }

//        /// <summary>
//        /// Gets current background color.
//        /// </summary>
//        public Color4 BackgroundColor
//        {
//            get { return m_background; }
//        }
//    }
//}
