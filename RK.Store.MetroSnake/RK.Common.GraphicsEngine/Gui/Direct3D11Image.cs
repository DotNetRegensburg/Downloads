using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RK.Common.GraphicsEngine.Core;
using RK.Common.GraphicsEngine.Drawing3D;

using D3D11 = SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;

namespace RK.Common.GraphicsEngine.Gui
{
    public class Direct3D11Image : Image
    {
        public static readonly DependencyProperty IsWireframeEnabledProperty =
            DependencyProperty.Register("IsWireframeEnabled", typeof(bool), typeof(Direct3D11Image), new PropertyMetadata(false));

        private HigherD3DImageSource m_d3dImageSource;
        //private Graphics3D m_graphics3D;
        private bool m_initialized;
        private DateTime m_lastRenderTime;
        private RenderState m_renderState;

        //All needed direct3d resources
        private D3D11.Texture2D m_backBufferForWpf;
        private D3D11.Texture2D m_backBufferD3D11;
        private D3D11.Texture2D m_depthBuffer;
        private D3D11.Device m_renderDevice;
        private D3D11.DeviceContext m_renderDeviceContext;
        private D3D11.RenderTargetView m_renderTarget;
        private D3D11.DepthStencilView m_renderTargetDepth;
        private DXGI.Surface m_renderTarget2DDxgi;

        //Some size related properties
        private int m_renderTargetHeight;
        private int m_renderTargetWidth;
        private int m_viewportHeight;
        private int m_viewportWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="Direct3D11Image"/> class.
        /// </summary>
        public Direct3D11Image()
        {
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;

            //Attach to SizeChanged event
            Observable.FromEventPattern<EventArgs>(this, "SizeChanged")
                .Throttle(TimeSpan.FromSeconds(0.5))
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe((eArgs) => OnThrottledSizeChanged());
        }

        [Category("Renderer")]
        public event Rendering3DHandler AfterRendering;

        [Category("Renderer")]
        public event Rendering3DHandler BeforeRendering;

        [Category("Renderer")]
        public event Updating3DHandler BeforeUpdating;

        public bool IsWireframeEnabled
        {
            get { return (bool)GetValue(IsWireframeEnabledProperty); }
            set { SetValue(IsWireframeEnabledProperty, value); }
        }

        /// <summary>
        /// Called when Direct3D rendering should be done.
        /// </summary>
        /// <param name="renderTarget">The render-target to use.</param>
        protected internal virtual void OnDirect3DPaint(RenderState renderState, UpdateState updateState)
        {

        }

        /// <summary>
        /// Called when the render size has changed.
        /// </summary>
        /// <param name="sizeInfo">New size information.</param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            //Resize render target only on greater size changes
            double resizeFactorWidth = sizeInfo.NewSize.Width > m_renderTargetWidth ? sizeInfo.NewSize.Width / m_renderTargetWidth : m_renderTargetWidth / sizeInfo.NewSize.Width;
            double resizeFactorHeight = sizeInfo.NewSize.Height > m_renderTargetHeight ? sizeInfo.NewSize.Height / m_renderTargetHeight : m_renderTargetHeight / sizeInfo.NewSize.Height;
            if ((resizeFactorWidth > 1.3) || (resizeFactorHeight > 1.3))
            {
                UnloadViewResources();
                CreateViewResources();
            }
        }

        /// <summary>
        /// Creates all view resources.
        /// </summary>
        private void CreateViewResources()
        {
            //Dispose resources of last swap chain
            UnloadViewResources();

            int width = Math.Max((int)base.ActualWidth, 100);
            int height = Math.Max((int)base.ActualHeight, 100);

            m_renderDevice = GraphicsCore.Current.HandlerD3D11.Device;
            m_renderDeviceContext = m_renderDevice.ImmediateContext;

            //Create the swap chain and the render target
            m_backBufferD3D11 = GraphicsHelper.CreateRenderTargetTexture(m_renderDevice, width, height);
            m_backBufferForWpf = GraphicsHelper.CreateSharedTexture(m_renderDevice, width, height);
            m_renderTarget = new D3D11.RenderTargetView(m_renderDevice, m_backBufferD3D11);

            //Create the depth buffer
            m_depthBuffer = GraphicsHelper.CreateDepthBufferTexture(m_renderDevice, width, height);
            m_renderTargetDepth = new D3D11.DepthStencilView(m_renderDevice, m_depthBuffer);

            //Apply render target size values
            m_renderTargetWidth = width;
            m_renderTargetHeight = height;

            //Recreate viewport
            RecreateViewPort();

            //Perform first render on the surface
            Render();

            //Set backbuffer to d3d image
            m_d3dImageSource.SetRenderTarget(m_backBufferForWpf);
        }

        private void Initialize()
        {
            //Create the d3d image
            m_d3dImageSource = new HigherD3DImageSource();
            m_d3dImageSource.IsFrontBufferAvailableChanged += OnD3DImageIsFrontBufferAvailableChanged;
            if (m_d3dImageSource.IsFrontBufferAvailable)
            {
                CompositionTarget.Rendering += OnCompositionTargetRendering;
            }

            //Create view resources
            CreateViewResources();

            //Enable 3d rendering
            m_initialized = true;
            this.Source = m_d3dImageSource;
        }

        /// <summary>
        /// Is this object in design mode?
        /// </summary>
        /// <param name="dependencyObject">The object to check.</param>
        private bool IsInDesignMode()
        {
            return DesignerProperties.GetIsInDesignMode(this);
        }

        /// <summary>
        /// Called when the composition target is rendering.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnCompositionTargetRendering(object sender, EventArgs e)
        {
            if (m_initialized && (m_renderDevice != null) && (this.ActualWidth > 0) && (this.ActualHeight > 0))
            {
                Render();
            }
            else
            {
                //Nothing to do
            }
        }

        /// <summary>
        /// Called when the availability of the front buffer of the d3d image has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnD3DImageIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (m_d3dImageSource.IsFrontBufferAvailable)
            {
                CompositionTarget.Rendering += OnCompositionTargetRendering;
            }
            else
            {
                CompositionTarget.Rendering -= OnCompositionTargetRendering;
            }
        }

        /// <summary>
        /// Called when the image is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!this.IsInDesignMode())
            {
                Initialize();
            }
        }

        /// <summary>
        /// Called when size changed event occurred.
        /// </summary>
        private void OnThrottledSizeChanged()
        {
            UnloadViewResources();
            CreateViewResources();
        }

        /// <summary>
        /// Called when the image is unloaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (!this.IsInDesignMode())
            {
                CompositionTarget.Rendering -= OnCompositionTargetRendering;

                //Disable rendering
                m_initialized = false;
                this.Source = null;

                //Clear d3d image
                m_d3dImageSource.IsFrontBufferAvailableChanged -= OnD3DImageIsFrontBufferAvailableChanged;
                GraphicsHelper.SafeDispose(ref m_d3dImageSource);

                //Clears view resources
                UnloadViewResources();
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
        /// Recreates current viewport.
        /// </summary>
        private void RecreateViewPort()
        {
            int width = Math.Max((int)base.ActualWidth, 100);
            int height = Math.Max((int)base.ActualHeight, 100);

            //Define the viewport for rendering
            D3D11.Viewport viewPort = GraphicsHelper.CreateDefaultViewport(width, height);

            //Set viewport and render target on the device
            if (m_renderState == null)
            {
                m_renderState = new RenderState(
                    m_renderDevice,
                    m_renderDeviceContext,
                    m_renderTarget, m_renderTargetDepth, viewPort,
                    Matrix4.Identity);
            }
            else
            {
                m_renderState.Reset(
                    m_renderTarget, m_renderTargetDepth, viewPort,
                    Matrix4.Identity);
            }

            //Apply new width and height values of the viewport
            m_viewportWidth = width;
            m_viewportHeight = height;
        }

        /// <summary>
        /// Performs rendering.
        /// </summary>
        private void Render()
        {
            if (m_d3dImageSource == null) { Initialize(); }
            if (!m_d3dImageSource.HasRenderTarget) { return; }
            
            m_d3dImageSource.Lock();
            try
            {
                //Set default rastarization state
                D3D11.RasterizerState rasterState = null;
                bool wireframeEnabled = this.IsWireframeEnabled;
                if (wireframeEnabled)
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
                DateTime currentTime = DateTime.UtcNow;
                TimeSpan updateTime = currentTime - m_lastRenderTime;
                if (updateTime.TotalMilliseconds > 100.0) { updateTime = TimeSpan.FromMilliseconds(100.0); }
                m_lastRenderTime = currentTime;
                UpdateState updateState = new UpdateState(updateTime);

                //Apply current target
                m_renderState.ApplyCurrentTarget();
                m_renderState.ApplyMaterial(null);

                //Paint using Direct3D
                m_renderDeviceContext.ClearRenderTargetView(m_renderTarget, new SharpDX.Color(0, 0, 0, 0));
                m_renderDeviceContext.ClearDepthStencilView(m_renderTargetDepth, D3D11.DepthStencilClearFlags.Depth | D3D11.DepthStencilClearFlags.Stencil, 1f, 0);

                //Raise events
                RaiseBeforeUpdating(updateState);
                RaiseBeforeRendering();

                //Call render methods of subclasses
                OnDirect3DPaint(m_renderState, updateState);

                //Raise AfterRendering event
                RaiseAfterRendering();

                //Clear current state after rendering
                m_renderState.ClearState();

                if (wireframeEnabled)
                {
                    m_renderDeviceContext.Rasterizer.State = null;
                    rasterState.Dispose();
                }

                ////Raises the TextureChanged event
                //m_backBufferSource.RaiseTextureChanged(m_renderState);

                //Copy contents of direct3D 11 texture to wpf texture. This step makes following possible
                // => Move all rendering logic to a background thread and perform only following on gui thread
                //m_renderDeviceContext.CopyResource(m_backBufferD3D11, m_backBufferForWpf);
                m_renderDeviceContext.ResolveSubresource(m_backBufferD3D11, 0, m_backBufferForWpf, 0, DXGI.Format.B8G8R8A8_UNorm);
                m_renderDeviceContext.Flush();

                m_d3dImageSource.AddDirtyRect(new Int32Rect(0, 0, m_d3dImageSource.PixelWidth, m_d3dImageSource.PixelHeight));
            }
            finally
            {
                m_d3dImageSource.Unlock();
            }
        }
        /// <summary>
        /// Unloads all view resources.
        /// </summary>
        private void UnloadViewResources()
        {
            if (m_renderDevice != null)
            {
                m_renderState = GraphicsHelper.DisposeGraphicsObject(m_renderState);
                m_renderTarget2DDxgi = GraphicsHelper.DisposeGraphicsObject(m_renderTarget2DDxgi);
                m_renderTargetDepth = GraphicsHelper.DisposeGraphicsObject(m_renderTargetDepth);
                m_depthBuffer = GraphicsHelper.DisposeGraphicsObject(m_depthBuffer);
                m_renderTarget = GraphicsHelper.DisposeGraphicsObject(m_renderTarget);
                m_backBufferForWpf = GraphicsHelper.DisposeGraphicsObject(m_backBufferForWpf);
                m_backBufferD3D11 = GraphicsHelper.DisposeGraphicsObject(m_backBufferD3D11);

                m_renderDevice = null;
            }
        }
    }
}