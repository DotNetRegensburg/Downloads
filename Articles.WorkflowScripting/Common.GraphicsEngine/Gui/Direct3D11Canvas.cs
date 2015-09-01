using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Common.GraphicsEngine.Core;
using Common.GraphicsEngine.Drawing3D;
using Common.GraphicsEngine.Drawing3D.Resources;
using D3D11 = SlimDX.Direct3D11;
//Some namespace mappings
using DXGI = SlimDX.DXGI;

namespace Common.GraphicsEngine.Gui
{
    public class Direct3D11Canvas : Control
    {
        //Resources for DXGI (DirectX Graphics Interface)
        private DXGI.Factory m_factory;
        private DXGI.SwapChain m_swapChain;
        private DXGI.Surface m_renderTarget2DDxgi;

        //Resources for Direct3D 11 (Used for 3D rendering)
        private bool m_initialized;
        private Exception m_initializationException;
        private D3D11.Device m_renderDevice;
        private D3D11.DeviceContext m_renderDeviceContext;
        private BackbufferCopiedTextureSource m_backBufferSource;
        private D3D11.RenderTargetView m_renderTarget;
        private D3D11.DepthStencilView m_renderTargetDepth;
        private D3D11.Texture2D m_backBuffer;
        private D3D11.Texture2D m_depthBuffer;

        //Generic members
        private RenderState m_renderState;
        private DateTime m_lastRenderTime;
        private Brush m_backBrush;
        private Timer m_renderTimer;
        private bool m_renderTimerInvokedRendering;
        private bool m_wireframeMode;

        [Category("Renderer")]
        public event Updating3DHandler BeforeUpdating;

        [Category("Renderer")]
        public event Rendering3DHandler BeforeRendering;

        [Category("Renderer")]
        public event Rendering3DHandler AfterRendering;

        /// <summary>
        /// Initializes a new instance of the <see cref="Direct3D11Panel"/> class.
        /// </summary>
        public Direct3D11Canvas()
        {
            //Set style parameters for this control
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
            base.SetStyle(ControlStyles.Opaque, true);
            base.DoubleBuffered = false;

            ////Create the scene and camera
            //m_camera = new Camera(this.Width, this.Height);
            //m_scene = new Scene(m_camera);

            ////Set standard values
            //m_isMovementEnabled = true;

            //Initialize background brush
            m_backBrush = new SolidBrush(this.BackColor);

            ////Create performance analyzers
            //m_renderSpeedAnalyzer = new CyclicSpeedAnalyzer("[Direct3D11Panel] Rendering");

            //Preparse backbuffer source for CopiedTextureResources
            m_backBufferSource = new BackbufferCopiedTextureSource(this);

            //Prepare render time object
            m_renderTimer = new Timer();
            m_renderTimer.Interval = 25;
            m_renderTimer.Tick += OnRenderTimerTick;
        }

        /// <summary>
        /// Saves a screenshot to harddisc.
        /// </summary>
        /// <param name="targetFile">Target file path.</param>
        /// <param name="fileFormat">Target file format.</param>
        public void SaveScreenshot(string targetFile, D3D11.ImageFileFormat fileFormat)
        {
            if (m_backBuffer != null)
            {
                D3D11.Texture2D.ToFile(
                    m_renderDeviceContext,
                    m_backBuffer,
                    fileFormat,
                    targetFile);
            }
        }

        /// <summary>
        /// Creates all view resources.
        /// </summary>
        private void CreateViewResources()
        {
            //Dispose resources of last swap chain
            UnloadViewResources();

            //Create the swap chain and the render target
            m_swapChain = GraphicsHelper.CreateDefaultSwapChain(this, m_factory, m_renderDevice);
            m_backBuffer = D3D11.Texture2D.FromSwapChain<D3D11.Texture2D>(m_swapChain, 0);
            m_renderTarget = new D3D11.RenderTargetView(m_renderDevice, m_backBuffer);

            //Create the depth buffer
            m_depthBuffer = GraphicsHelper.CreateDepthBufferTexture(m_renderDevice, this.Width, this.Height);
            m_renderTargetDepth = new D3D11.DepthStencilView(m_renderDevice, m_depthBuffer);

            //Define the viewport for rendering
            D3D11.Viewport viewPort = GraphicsHelper.CreateDefaultViewport(this);

            //Set viewport and render target on the device
            m_renderState = new RenderState(
                m_renderDevice,
                m_renderDeviceContext,
                m_renderTarget, m_renderTargetDepth, viewPort,
                Matrix4.Identity);
        }

        /// <summary>
        /// Recreates all view resources.
        /// </summary>
        private void RecreateViewResources()
        {
            UnloadViewResources();

            if ((this.Width > 0) && (this.Height > 0))
            {
                CreateViewResources();
            }
        }

        /// <summary>
        /// Unloads all view resources.
        /// </summary>
        private void UnloadViewResources()
        {
            m_renderTarget2DDxgi = GraphicsHelper.DisposeGraphicsObject(m_renderTarget2DDxgi);
            m_renderTargetDepth = GraphicsHelper.DisposeGraphicsObject(m_renderTargetDepth);
            m_depthBuffer = GraphicsHelper.DisposeGraphicsObject(m_depthBuffer);
            m_renderTarget = GraphicsHelper.DisposeGraphicsObject(m_renderTarget);
            m_backBuffer = GraphicsHelper.DisposeGraphicsObject(m_backBuffer);
            m_swapChain = GraphicsHelper.DisposeGraphicsObject(m_swapChain);
        }

        /// <summary>
        /// Initialize 3d graphics with direct3D 11.
        /// </summary>
        public void Initialize3D()
        {
            try
            {
                //Get all factories
                m_factory = GraphicsCore.Current.HandlerDXGI.Factory;

                //Get all devices
                m_renderDevice = GraphicsCore.Current.HandlerD3D11.Device;
                m_renderDeviceContext = m_renderDevice.ImmediateContext;

                //Creates swap chain and render target
                if ((this.Width > 0) && (this.Height > 0))
                {
                    CreateViewResources();
                }

                //Update local init-flag
                m_initialized = true;
                m_initializationException = null;
                this.DoubleBuffered = false;

                //Remember current time
                m_lastRenderTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                UnloadViewResources();

                //Update local init-flag
                // -> Error message is displayed in OnPaint method
                m_initialized = false;
                m_initializationException = ex;
                this.DoubleBuffered = true;
            }
        }

        /// <summary>
        /// Unload 3d graphics.
        /// </summary>
        private void Unload3D()
        {
            if (m_initialized)
            {
                ////Unload all resources of the scene
                //m_scene.UnloadResources();

                //Unload all view resources
                UnloadViewResources();

                //Update initialization flags
                m_initialized = false;
            }
        }

        /// <summary>
        /// Called when Direct2D rendering should be done.
        /// </summary>
        /// <param name="renderTarget">The render-target to use.</param>
        protected internal virtual void OnDirect3DPaint(RenderState renderState, UpdateState updateState)
        {
            //renderTarget.BeginDraw();
            //try
            //{
            //    renderTarget.Clear(new Color4(this.BackColor));

            //    //Call external painting
            //    RaiseDirect2DPaint(m_graphics);
            //}
            //finally
            //{
            //    renderTarget.EndDraw();
            //}
        }

        /// <summary>
        /// Called when system wants to paint this panel.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (m_initialized && (this.Width > 0) && (this.Height > 0))
            {
                //Just render current clipping area
                m_renderDeviceContext.Rasterizer.SetScissorRectangles(e.ClipRectangle);

                //Set default rastarization state
                D3D11.RasterizerState rasterState = null;
                if (m_wireframeMode)
                {
                    rasterState = D3D11.RasterizerState.FromDescription(m_renderDevice, new D3D11.RasterizerStateDescription()
                    {
                        CullMode = D3D11.CullMode.Back,
                        FillMode = D3D11.FillMode.Wireframe,
                        IsFrontCounterclockwise = false,
                        DepthBias = 0,
                        SlopeScaledDepthBias = 0f,
                        DepthBiasClamp = 0f,
                        IsDepthClipEnabled = true,
                        IsAntialiasedLineEnabled = false,
                        IsMultisampleEnabled = false,
                        IsScissorEnabled = false
                    });
                    m_renderDeviceContext.Rasterizer.State = rasterState;
                }

                //Get update time
                DateTime currentTime = DateTime.Now;
                TimeSpan updateTime = currentTime - m_lastRenderTime;
                if (updateTime.TotalMilliseconds > 100.0) { updateTime = TimeSpan.FromMilliseconds(100.0); }
                m_lastRenderTime = currentTime;
                UpdateState updateState = new UpdateState(updateTime);

                //Apply current target
                m_renderState.ApplyCurrentTarget();

                //Paint using Direct3D
                m_renderDeviceContext.ClearRenderTargetView(m_renderTarget, this.BackColor);//this.BackColor);
                m_renderDeviceContext.ClearDepthStencilView(m_renderTargetDepth, D3D11.DepthStencilClearFlags.Depth, 1f, 0);

                //Raise events
                RaiseBeforeUpdating(updateState);
                RaiseBeforeRendering();

                //Call render methods of subclasses
                OnDirect3DPaint(m_renderState, updateState);

                //Raise AfterRendering event
                RaiseAfterRendering();

                //Present all rendered stuff on screen
                m_swapChain.Present(0, DXGI.PresentFlags.None);

                //Clear current state after rendering
                m_renderState.ClearState();

                //Raises the TextureChanged event
                m_backBufferSource.RaiseTextureChanged(m_renderState);

                if (m_wireframeMode)
                {
                    m_renderDeviceContext.Rasterizer.State = null;
                    rasterState.Dispose();
                }
            }
            else
            {
                //Paint using System.Drawing
                e.Graphics.FillRectangle(m_backBrush, e.ClipRectangle);

                //Display initialization exception (if any)
                if (m_initializationException != null)
                {
                    e.Graphics.DrawString("Error during initialization of 3D graphics!", this.Font, Brushes.Black, new PointF(10f, 10f));
                    e.Graphics.DrawString(m_initializationException.Message, this.Font, Brushes.Black, new PointF(10f, 25f));
                }
            }

            m_renderTimerInvokedRendering = false;
        }

        /// <summary>
        /// Called when BackColor property has changed.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);

            //Update background brush
            if (m_backBrush != null) { m_backBrush.Dispose(); }
            m_backBrush = new SolidBrush(this.BackColor);
        }

        /// <summary>
        /// Called when size 
        /// </summary>
        /// <param name="eventargs"></param>
        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);

            if ((!this.DesignMode) && m_initialized)
            {
                if ((this.Width > 0) && (this.Height > 0))
                {
                    RecreateViewResources();
                }
            }
        }

        /// <summary>
        /// Called when render timer ticks.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnRenderTimerTick(object sender, EventArgs e)
        {
            if (!m_renderTimerInvokedRendering)
            {
                m_renderTimerInvokedRendering = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Called when the window handle is created.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if ((!this.DesignMode) && (!m_initialized))
            {
                //Initialize 3d graphics after panel creation
                Initialize3D();
            }
        }

        /// <summary>
        /// Called when the window handle is destroyed.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);

            if ((!this.DesignMode) && m_initialized)
            {
                //Unload 3d graphics 
                Unload3D();
            }
        }

        /// <summary>
        /// Raises the BeforeUpdating event.
        /// </summary>
        private void RaiseBeforeUpdating(UpdateState updateState)
        {
            if (BeforeUpdating != null) { BeforeUpdating(this, new Updating3DArgs(updateState)); }
        }

        /// <summary>
        /// Raises the BeforeRendering event.
        /// </summary>
        private void RaiseBeforeRendering()
        {
            if (BeforeRendering != null) { BeforeRendering(this, new Rendering3DArgs(m_renderState)); }
        }

        /// <summary>
        /// Raises the AfterRendering event.
        /// </summary>
        private void RaiseAfterRendering()
        {
            if (AfterRendering != null) { AfterRendering(this, new Rendering3DArgs(m_renderState)); }
        }

        /// <summary>
        /// Is graphics system initialized?
        /// </summary>
        [Browsable(false)]
        public bool Initialized
        {
            get { return m_initialized; }
        }

        /// <summary>
        /// Gets the back buffer source.
        /// </summary>
        public ICopiedTextureSource BackBufferSource
        {
            get { return m_backBufferSource; }
        }

        /// <summary>
        /// Is wireframe enabled?
        /// </summary>
        [Category("Rendering")]
        public bool IsWireframeEnabled
        {
            get { return m_wireframeMode; }
            set
            {
                if (m_wireframeMode != value)
                {
                    m_wireframeMode = value;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Is cyclic rendering enabled?
        /// </summary>
        [Category("Rendering")]
        [DefaultValue(false)]
        public bool CyclicRendering
        {
            get { return m_renderTimer.Enabled; }
            set
            {
                //if (this.DesignMode) { return; }

                if (value != CyclicRendering)
                {
                    if (!m_renderTimer.Enabled) { m_renderTimer.Start(); }
                    else { m_renderTimer.Stop(); }
                }
            }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        private class BackbufferCopiedTextureSource : ICopiedTextureSource
        {
            private Direct3D11Canvas m_owner;

            public event TextureChangedHandler TextureChanged;

            /// <summary>
            /// Initializes a new instance of the <see cref="BackbufferCopiedTextureSource"/> class.
            /// </summary>
            /// <param name="owner">The owner.</param>
            public BackbufferCopiedTextureSource(Direct3D11Canvas owner)
            {
                m_owner = owner;
            }

            /// <summary>
            /// Raises the TextureChanged event.
            /// </summary>
            /// <param name="renderState">Current render state.</param>
            public void RaiseTextureChanged(RenderState renderState)
            {
                if (TextureChanged != null) { TextureChanged(this, new TextureChangedEventArgs(renderState)); }
            }

            /// <summary>
            /// Gets the texture.
            /// </summary>
            public D3D11.Texture2D Texture
            {
                get { return m_owner.m_backBuffer; }
            }

            /// <summary>
            /// Gets the width of the texture.
            /// </summary>
            public int TextureWidth
            {
                get { return m_owner.Width; }
            }

            /// <summary>
            /// Gets the height of the texture.
            /// </summary>
            public int TextureHeight
            {
                get { return m_owner.Height; }
            }
        }
    }
}
