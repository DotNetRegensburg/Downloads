using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

//Same namespace mappings.
using D3D11 = SlimDX.Direct3D11;

namespace Common.GraphicsEngine.Drawing3D
{
    public class RenderState
    {
        //Resources for Direct3D 11 rendering
        private D3D11.Device m_device;
        private D3D11.DeviceContext m_deviceContext;

        //Generic fields
        private Stack<RenderStackEntry> m_renderSettingsStack;
        private Stack<Scene> m_sceneStack;
        private RenderStackEntry m_currentRenderSettings;
        private Scene m_currentScene;
        private Matrix4Stack m_world;
        private Graphics3D m_graphics;

        ////Current state
        //private MaterialResource m_currentMaterial;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderState"/> class.
        /// </summary>
        private RenderState(D3D11.Device device, D3D11.DeviceContext deviceContext)
        {
            //Set device members
            m_device = device;
            m_deviceContext = deviceContext;

            //Initialize world matrix
            m_world = new Matrix4Stack(Matrix4.Identity);

            //Create settings stack
            m_renderSettingsStack = new Stack<RenderStackEntry>();
            m_sceneStack = new Stack<Scene>();

            //Creates a new graphics object
            //TODO: Move this to another place (for example the Canvas control)
            m_graphics = new Graphics3D(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderState"/> class.
        /// </summary>
        /// <param name="device">The device object.</param>
        /// <param name="deviceContext">The device context.</param>
        /// <param name="renderTargetView">The render target view.</param>
        /// <param name="depthStencilView">The depth stencil view.</param>
        /// <param name="viewport">The viewport.</param>
        /// <param name="viewProj">The ViewProjection matrix.</param>
        public RenderState(
            D3D11.Device device,
            D3D11.DeviceContext deviceContext,
            D3D11.RenderTargetView renderTargetView,
            D3D11.DepthStencilView depthStencilView,
            D3D11.Viewport viewport,
            Matrix4 viewProj)
            : this(device, deviceContext)
        {
            //Inititialize current render properties
            m_currentRenderSettings = new RenderStackEntry();
            m_currentRenderSettings.IsSingle = true;
            m_currentRenderSettings.MatrixStack = new Matrix4Stack();
            m_currentRenderSettings.SingleDepthStencilView = depthStencilView;
            m_currentRenderSettings.SingleRenderTargetView = renderTargetView;
            m_currentRenderSettings.SingleViewport = viewport;
            m_currentRenderSettings.ViewProj = viewProj;

            //Apply initial render properties
            m_currentRenderSettings.Apply(m_deviceContext);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderState"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="deviceContext">The device context.</param>
        /// <param name="renderTargetViews">The render target views.</param>
        /// <param name="depthStencilView">The depth stencil view.</param>
        /// <param name="viewports">The viewports.</param>
        /// <param name="viewProj">The ViewProjection matrix.</param>
        public RenderState(
            D3D11.Device device,
            D3D11.DeviceContext deviceContext,
            D3D11.RenderTargetView[] renderTargetViews,
            D3D11.DepthStencilView depthStencilView,
            D3D11.Viewport[] viewports,
            Matrix4 viewProj)
            : this(device, deviceContext)
        {
            //Inititialize current render properties
            m_currentRenderSettings = new RenderStackEntry();
            m_currentRenderSettings.IsSingle = false;
            m_currentRenderSettings.MatrixStack = new Matrix4Stack();
            m_currentRenderSettings.SingleDepthStencilView = depthStencilView;
            m_currentRenderSettings.MultiRenderTargetViews = renderTargetViews;
            m_currentRenderSettings.MultiViewports = viewports;
            m_currentRenderSettings.ViewProj = viewProj;

            //Apply initial render properties
            m_deviceContext.Rasterizer.SetViewports(viewports);
            m_deviceContext.OutputMerger.SetTargets(depthStencilView, renderTargetViews);
        }

        /// <summary>
        /// Applies current render target settings.
        /// </summary>
        public void ClearState()
        {
            m_deviceContext.ClearState();
            if (m_currentRenderSettings != null) { m_currentRenderSettings.Apply(m_deviceContext); }
        }

        /// <summary>
        /// Clears current depth buffer.
        /// </summary>
        public void ClearCurrentDepthBuffer()
        {
            if ((m_currentRenderSettings != null) && (m_currentRenderSettings.SingleDepthStencilView != null))
            {
                m_deviceContext.ClearDepthStencilView(
                    m_currentRenderSettings.SingleDepthStencilView,
                    D3D11.DepthStencilClearFlags.Depth,
                    1f, 0);
            }
        }

        /// <summary>
        /// Pushes a scene onto the stack.
        /// </summary>
        /// <param name="scene">Scene to push.</param>
        public void PushScene(Scene scene)
        {
            m_sceneStack.Push(m_currentScene);
            m_currentScene = scene;
        }

        /// <summary>
        /// Pops a scene from the stack.
        /// </summary>
        public void PopScene()
        {
            if (m_sceneStack.Count < 0) { throw new ApplicationException("There is only one element on the render stack!"); }

            m_currentScene = m_sceneStack.Pop();
        }

        /// <summary>
        /// Pushes a new render target onto the render target stack.
        /// </summary>
        /// <param name="renderTargetView">The RenderTargetView object.</param>
        /// <param name="renderTargetDepthView">The DepthStencilView object.</param>
        /// <param name="viewport">The viewport.</param>
        public void PushRenderTarget(
            D3D11.RenderTargetView renderTargetView,
            D3D11.DepthStencilView renderTargetDepthView,
            D3D11.Viewport viewport,
            Matrix4 viewProjection)
        {
            //Build new render stack entry
            RenderStackEntry newEntry = new RenderStackEntry();
            newEntry.MatrixStack = new Matrix4Stack();
            newEntry.ViewProj = viewProjection;
            newEntry.IsSingle = true;
            newEntry.SingleDepthStencilView = renderTargetDepthView;
            newEntry.SingleRenderTargetView = renderTargetView;
            newEntry.SingleViewport = viewport;

            //Overtake device settings
            newEntry.Apply(m_deviceContext);

            //Push new entry onto the stack
            m_renderSettingsStack.Push(m_currentRenderSettings);
            m_currentRenderSettings = newEntry;
        }

        /// <summary>
        /// Pushes a new render target onto the render target stack.
        /// </summary>
        /// <param name="renderTargetViews">All RenderTargetView objects.</param>
        /// <param name="depthStencilView">The DepthStencilView object.</param>
        /// <param name="viewports">The viewport object.</param>
        public void PushRenderTarget(
            D3D11.RenderTargetView[] renderTargetViews,
            D3D11.DepthStencilView depthStencilView,
            D3D11.Viewport[] viewports,
            Matrix4 viewProjection)
        {
            //Build new render stack entry
            RenderStackEntry newEntry = new RenderStackEntry();
            newEntry.MatrixStack = new Matrix4Stack();
            newEntry.ViewProj = viewProjection;
            newEntry.IsSingle = false;
            newEntry.SingleDepthStencilView = depthStencilView;
            newEntry.MultiRenderTargetViews = renderTargetViews;
            newEntry.MultiViewports = viewports;

            //Overtake device settings
            newEntry.Apply(m_deviceContext);

            //Push new entry onto the stack
            m_renderSettingsStack.Push(m_currentRenderSettings);
            m_currentRenderSettings = newEntry;
        }

        /// <summary>
        /// Pops a render target from the render target stack.
        /// </summary>
        public void PopRenderTarget()
        {
            if (m_renderSettingsStack.Count < 1) { throw new ApplicationException("There is only one element on the render stack!"); }

            //Pop last entry
            m_currentRenderSettings = m_renderSettingsStack.Pop();

            //Apply old configuration
            m_currentRenderSettings.Apply(m_deviceContext);
        }

        /// <summary>
        /// Applies current target settings.
        /// </summary>
        public void ApplyCurrentTarget()
        {
            if (m_currentRenderSettings != null)
            {
                m_currentRenderSettings.Apply(m_deviceContext);
            }
        }

        /// <summary>
        /// Generates a WorldViewProjection matrix.
        /// </summary>
        /// <param name="world">The world matrix.</param>
        public Matrix4 GenerateWorldViewProj(Matrix4 world)
        {
            return world * m_currentRenderSettings.ViewProj;
        }

        /// <summary>
        /// Gets current Device object.
        /// </summary>
        public D3D11.Device Device
        {
            get { return m_device; }
        }

        /// <summary>
        /// Gets current DeviceContext object.
        /// </summary>
        public D3D11.DeviceContext DeviceContext
        {
            get { return m_deviceContext; }
        }

        /// <summary>
        /// Gets the ViewProj matrix.
        /// </summary>
        public Matrix4 ViewProj
        {
            get { return m_currentRenderSettings.ViewProj; }
            set { m_currentRenderSettings.ViewProj = value; }
        }

        /// <summary>
        /// Gets current world matrix.
        /// </summary>
        /// <value>The world.</value>
        public Matrix4Stack World
        {
            get { return m_world; }
        }

        /// <summary>
        /// Gets current scene object.
        /// </summary>
        public Scene Scene
        {
            get { return m_currentScene; }
        }

        /// <summary>
        /// Gets current camera.
        /// </summary>
        public Camera Camera
        {
            get
            {
                if (m_currentScene != null) { return m_currentScene.MainCamera; }
                else { return null; }
            }
        }

        /// <summary>
        /// Gets current graphics object.
        /// </summary>
        public Graphics3D Graphics
        {
            get { return m_graphics; }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        /// <summary>
        /// HelperClass for RenderState class
        /// </summary>
        class RenderStackEntry
        {
            public Matrix4Stack MatrixStack;
            public Matrix4 ViewProj;
            public D3D11.RenderTargetView[] MultiRenderTargetViews;
            public D3D11.Viewport[] MultiViewports;
            public D3D11.RenderTargetView SingleRenderTargetView;
            public D3D11.DepthStencilView SingleDepthStencilView;
            public D3D11.Viewport SingleViewport;
            public bool IsSingle;

            /// <summary>
            /// Applies all properties.
            /// </summary>
            /// <param name="deviceContext">Target DeviceContext object.</param>
            public void Apply(D3D11.DeviceContext deviceContext)
            {
                if (IsSingle)
                {
                    deviceContext.Rasterizer.SetViewports(SingleViewport);
                    deviceContext.OutputMerger.SetTargets(SingleDepthStencilView, SingleRenderTargetView);
                }
                else
                {
                    deviceContext.Rasterizer.SetViewports(MultiViewports);
                    deviceContext.OutputMerger.SetTargets(SingleDepthStencilView, MultiRenderTargetViews);
                }
            }
        }
    }
}
