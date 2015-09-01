using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.Graphics.Display;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using RK.Common;
using RK.Common.GraphicsEngine.Core;
using RK.Common.GraphicsEngine.Drawing3D;
using RK.Common.GraphicsEngine.Drawing2D;

//Some namespace mappings
using DXGI = SharpDX.DXGI;
using D3D11 = SharpDX.Direct3D11;
using D2D = SharpDX.Direct2D1;

namespace RK.Common.GraphicsEngine.Gui
{
    public class BackgroundPanelDirectXCanvas : IDisposable
    {
        private D3D11.Texture2D m_backBuffer;
        private float m_currentDpi = DisplayProperties.LogicalDpi;
        private D2D.DeviceContext m_d2dContext;
        private D3D11.Texture2D m_depthBuffer;
        private bool m_discardRendering;
        private DXGI.Factory2 m_factory;
        //Resources for Direct2D rendering
        private Graphics2DCache m_graphics2DCache;

        private Exception m_initializationException;
        //Resources for Direct3D 11 (Used for 3D rendering)
        private bool m_initialized;

        private bool m_isDisposed;
        private DateTime m_lastRenderTime;
        private D3D11.Device m_renderDevice;
        private D3D11.DeviceContext m_renderDeviceContext;
        private RenderState m_renderState;
        private RenderState2D m_renderState2D;
        private D3D11.RenderTargetView m_renderTarget;
        private D3D11.DepthStencilView m_renderTargetDepth;
        private DXGI.SwapChain1 m_swapChain;
        private SwapChainBackgroundPanel m_targetPanel;
        private DXGI.ISwapChainBackgroundPanelNative m_targetPanelInterface;
        private int m_totalRenderCount;
        private bool m_wireframeMode;
        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundPanelDirectXCanvas" /> class.
        /// </summary>
        /// <param name="targetPanel">The target panel.</param>
        public BackgroundPanelDirectXCanvas(SwapChainBackgroundPanel targetPanel)
        {

            m_targetPanel = targetPanel;
            m_targetPanelInterface = ComObject.As<DXGI.ISwapChainBackgroundPanelNative>(targetPanel);

            Initialize3D();

            targetPanel.SizeChanged += OnTargetPanelSizeChanged;

            //Start render loop (trigger rendering in 30 ms interval)
            targetPanel.InvokeDelayedWhile(
                () => true,
                () => Render(),
                TimeSpan.FromMilliseconds(15.0),
                () => { },
                InvokeDelayedMode.EnsuredTimerInterval);

            //Define unloading behavior
            targetPanel.Unloaded += (sender, eArgs) =>
            {
                this.Dispose();
                if (m_graphics2DCache != null)
                {
                    m_graphics2DCache.Dispose();
                    m_graphics2DCache = null;
                }
            };
        }

        public event Rendering3DHandler AfterRendering;

        public event Rendering3DHandler BeforeRendering;

        public event Updating3DHandler BeforeUpdating;
        /// <summary>
        /// Discard rendering?
        /// </summary>
        public bool DiscardRendering
        {
            get { return m_discardRendering; }
            set { m_discardRendering = value; }
        }

        /// <summary>
        /// Height of the swap chain to create or resize.
        /// </summary>
        public int Height
        {
            get
            {
                return (int)(m_targetPanel.RenderSize.Height * m_currentDpi / 96.0);
            }
        }

        /// <summary>
        /// Gets current SwapChain object.
        /// </summary>
        public DXGI.SwapChain1 SwapChain
        {
            get { return m_swapChain; }
        }

        /// <summary>
        /// Gets the total count of render operations.
        /// </summary>
        public int TotalRenderCount
        {
            get { return m_totalRenderCount; }
        }

        /// <summary>
        /// Width of the swap chain to create or resize.
        /// </summary>
        public int Width
        {
            get
            {
                return (int)(m_targetPanel.RenderSize.Width * m_currentDpi / 96.0);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!m_isDisposed)
            {
                //Dispsoe all resources

                m_isDisposed = true;
            }
        }

        /// <summary>
        /// Renders all contents of the screen
        /// </summary>
        public void Render()
        {
            if (m_isDisposed) { return; }
            if (m_discardRendering) { return; }

            if (m_initialized && (this.Width > 0) && (this.Height > 0))
            {
                //Set default rastarization state
                D3D11.RasterizerState rasterState = null;
                if (m_wireframeMode)
                {
                    rasterState = new D3D11.RasterizerState(m_renderDevice, new D3D11.RasterizerStateDescription()
                    {
                        CullMode = D3D11.CullMode.Back,
                        FillMode = D3D11.FillMode.Wireframe,
                        IsFrontCounterClockwise = false,
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
                m_renderDeviceContext.ClearRenderTargetView(m_renderTarget, new SharpDX.Color4(Color4.CornflowerBlue.ToRgba()));//this.BackColor);
                m_renderDeviceContext.ClearDepthStencilView(m_renderTargetDepth, D3D11.DepthStencilClearFlags.Depth, 1f, 0);

                //Raise events
                RaiseBeforeUpdating(updateState);
                RaiseBeforeRendering();

                //Call render methods of subclasses
                OnDirect3DPaint(m_renderState, updateState);

                //Raise AfterRendering event
                RaiseAfterRendering();

                //Call 2d rendering after 3D render
                m_d2dContext.BeginDraw();
                try
                {
                    OnDirect2DPaintAfter3DRendering(m_renderState2D, updateState);
                }
                finally
                {
                    m_d2dContext.EndDraw();
                }

                //Present all rendered stuff on screen
                m_swapChain.Present(0, DXGI.PresentFlags.None);

                //Clear current state after rendering
                m_renderState.ClearState();

                ////Raises the TextureChanged event
                //m_backBufferSource.RaiseTextureChanged(m_renderState);

                if (m_wireframeMode)
                {
                    m_renderDeviceContext.Rasterizer.State = null;
                    rasterState.Dispose();
                }

                //Increment total render count if it is not on the maximum
                if (m_totalRenderCount < Int32.MaxValue)
                {
                    m_totalRenderCount++;
                }
            }

            //m_renderTimerInvokedRendering = false;
        }

        /// <summary>
        /// Called when Direct2D rendering should be done.
        /// </summary>
        /// <param name="renderState">The current render state.</param>
        /// <param name="updateState">Current update state.</param>
        protected internal virtual void OnDirect2DPaintAfter3DRendering(RenderState2D renderState, UpdateState updateState)
        {

        }

        /// <summary>
        /// Called when Direct3D rendering should be done.
        /// </summary>
        /// <param name="renderTarget">The render-target to use.</param>
        protected internal virtual void OnDirect3DPaint(RenderState renderState, UpdateState updateState)
        {

        }

        /// <summary>
        /// Called when the size of the target panel has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.SizeChangedEventArgs" /> instance containing the event data.</param>
        protected virtual void OnTargetPanelSizeChanged(object sender, SizeChangedEventArgs e)
        {
            RecreateViewResources();
        }

        /// <summary>
        /// Creates the swap chain.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="device">The device.</param>
        /// <param name="desc">The desc.</param>
        private DXGI.SwapChain1 CreateSwapChain(DXGI.Factory2 factory, D3D11.Device1 device, DXGI.SwapChainDescription1 desc)
        {
            //Creates the swap chain for XAML composition
            DXGI.SwapChain1 swapChain = factory.CreateSwapChainForComposition(device, ref desc, null);

            //Associate the SwapChainBackgroundPanel with the swap chain
            m_targetPanelInterface.SwapChain = swapChain;

            //Returns the new swap chain
            return swapChain;
        }

        /// <summary>
        /// Creates the swap chain description.
        /// </summary>
        /// <returns>A swap chain description</returns>
        private DXGI.SwapChainDescription1 CreateSwapChainDescription()
        {
            int qualityLevels = m_renderDevice.CheckMultisampleQualityLevels(DXGI.Format.B8G8R8A8_UNorm, 2);

            // SwapChain description
            var desc = new SharpDX.DXGI.SwapChainDescription1()
            {
                // Automatic sizing
                Width = Width,
                Height = Height,
                Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                Stereo = false,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                Usage = SharpDX.DXGI.Usage.BackBuffer | SharpDX.DXGI.Usage.RenderTargetOutput,
                BufferCount = 2,
                Scaling = SharpDX.DXGI.Scaling.Stretch,
                SwapEffect = SharpDX.DXGI.SwapEffect.FlipSequential,
            };
            return desc;
        }

        /// <summary>
        /// Creates all view resources.
        /// </summary>
        private void CreateViewResources()
        {
            //Dispose resources of last swap chain
            UnloadViewResources();

            //Create the swap chain and the render target
            m_swapChain = CreateSwapChain(
                GraphicsCore.Current.HandlerDXGI.Factory,
                GraphicsCore.Current.HandlerD3D11.Device,
                CreateSwapChainDescription());
            m_backBuffer = D3D11.Texture2D.FromSwapChain<D3D11.Texture2D>(m_swapChain, 0);
            m_renderTarget = new D3D11.RenderTargetView(m_renderDevice, m_backBuffer);

            //Create direct2d surface
            using (DXGI.Surface backbufferSurface = m_backBuffer.QueryInterface<DXGI.Surface>())
            {
                m_d2dContext = new D2D.DeviceContext(backbufferSurface);
            }
            if (m_graphics2DCache == null)
            {
                m_graphics2DCache = new Graphics2DCache(m_d2dContext);
            }

            //Create the depth buffer
            m_depthBuffer = GraphicsHelper.CreateDepthBufferTexture(m_renderDevice, this.Width, this.Height);
            m_renderTargetDepth = new D3D11.DepthStencilView(m_renderDevice, m_depthBuffer);

            //Define the viewport for rendering
            D3D11.Viewport viewPort = GraphicsHelper.CreateDefaultViewport(this.Width, this.Height);

            //Set viewport and render target on the device
            m_renderState = new RenderState(
                m_renderDevice,
                m_renderDeviceContext,
                m_renderTarget, m_renderTargetDepth, viewPort,
                Matrix4.Identity);
            m_renderState2D = new RenderState2D(m_d2dContext, m_graphics2DCache);
        }

        /// <summary>
        /// Initialize 3d graphics with direct3D 11.
        /// </summary>
        private void Initialize3D()
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

                //Remember current time
                m_lastRenderTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                UnloadViewResources();

                //Update local init-flag
                m_initialized = false;
                m_initializationException = ex;
            }
        }
        /// <summary>
        /// Raises the AfterRendering event.
        /// </summary>
        private void RaiseAfterRendering()
        {
            if (AfterRendering != null) { AfterRendering(this, new Rendering3DArgs(m_renderState)); }
        }

        /// <summary>
        /// Raises the BeforeRendering event.
        /// </summary>
        private void RaiseBeforeRendering()
        {
            if (BeforeRendering != null) { BeforeRendering(this, new Rendering3DArgs(m_renderState)); }
        }

        /// <summary>
        /// Raises the BeforeUpdating event.
        /// </summary>
        private void RaiseBeforeUpdating(UpdateState updateState)
        {
            if (BeforeUpdating != null) { BeforeUpdating(this, new Updating3DArgs(updateState)); }
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
            //m_renderTarget2DDxgi = GraphicsHelper.DisposeGraphicsObject(m_renderTarget2DDxgi);
            m_renderTargetDepth = GraphicsHelper.DisposeGraphicsObject(m_renderTargetDepth);
            m_depthBuffer = GraphicsHelper.DisposeGraphicsObject(m_depthBuffer);
            m_renderTarget = GraphicsHelper.DisposeGraphicsObject(m_renderTarget);
            m_backBuffer = GraphicsHelper.DisposeGraphicsObject(m_backBuffer);
            m_swapChain = GraphicsHelper.DisposeGraphicsObject(m_swapChain);
            m_d2dContext = GraphicsHelper.DisposeGraphicsObject(m_d2dContext);
        }
    }
}
