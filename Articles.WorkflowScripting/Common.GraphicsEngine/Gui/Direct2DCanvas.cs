using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SlimDX;
using Common;
using Common.GraphicsEngine.Drawing2D;

//Some namespace mappings
using GDI  = System.Drawing;
using D2D  = SlimDX.Direct2D;
using DXGI = SlimDX.DXGI;
using Common.GraphicsEngine.Core;

namespace Common.GraphicsEngine.Gui
{
    public partial class Direct2DCanvas : Control
    {
        private CanvasType m_canvasType;
        private GDI.Brush m_backgroundBrush;
        private D2D.WindowRenderTarget m_renderTarget;
        private Graphics2D m_graphics;

        /// <summary>
        /// Called when the canvas is about to paint all 2d contents.
        /// </summary>
        [Category("Rendering")]
        public event Direct2DPaintEventHandler Paint2D;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public Direct2DCanvas()
        {
            //No Direct2D rendering available while loading
            m_canvasType = CanvasType.None;

            //Create background-brush
            m_backgroundBrush = new SolidBrush(this.BackColor);

            //Set styles
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
            base.SetStyle(ControlStyles.Opaque, true);
            base.DoubleBuffered = false;
        }

        /// <summary>
        /// Called when the control's handle is created.
        /// </summary>
        protected override void OnHandleCreated(EventArgs e)
        {
            if (!GraphicsCore.IsInitialized) { GraphicsCore.Initialize(); }

            try
            {
                m_canvasType = CanvasType.None;

                //Create the render target
                if (GraphicsCore.IsInitialized)
                {
                    D2D.Factory factory = GraphicsCore.Current.HandlerD2D.Factory;

                    try
                    {
                        try
                        {
                            //Try to create render target using hardware
                            m_renderTarget = new D2D.WindowRenderTarget(
                                factory,
                                new D2D.RenderTargetProperties()
                                {
                                    MinimumFeatureLevel = D2D.FeatureLevel.Direct3D10,
                                    PixelFormat = new D2D.PixelFormat(
                                        DXGI.Format.B8G8R8A8_UNorm,
                                        D2D.AlphaMode.Premultiplied),
                                    Type = D2D.RenderTargetType.Hardware,
                                    Usage = D2D.RenderTargetUsage.ForceBitmapRemoting
                                },
                                new D2D.WindowRenderTargetProperties()
                                {
                                    Handle = this.Handle,
                                    PixelSize = this.Size,
                                    PresentOptions = D2D.PresentOptions.Immediately
                                });
                            m_graphics = new Graphics2D(m_renderTarget);
                        }
                        catch (Exception)
                        {
                            //Try to create render target using hardware
                            m_renderTarget = new D2D.WindowRenderTarget(
                                factory,
                                new D2D.RenderTargetProperties()
                                {
                                    MinimumFeatureLevel = D2D.FeatureLevel.Direct3D9,
                                    PixelFormat = new D2D.PixelFormat(
                                        DXGI.Format.B8G8R8A8_UNorm,
                                        D2D.AlphaMode.Premultiplied),
                                    Type = D2D.RenderTargetType.Hardware,
                                    Usage = D2D.RenderTargetUsage.ForceBitmapRemoting
                                },
                                new D2D.WindowRenderTargetProperties()
                                {
                                    Handle = this.Handle,
                                    PixelSize = this.Size,
                                    PresentOptions = D2D.PresentOptions.Immediately
                                });
                            m_graphics = new Graphics2D(m_renderTarget);
                        }
                        m_canvasType = CanvasType.Hardware;
                    }
                    catch (Exception)
                    {
                        //Create render target using software
                        m_renderTarget = new D2D.WindowRenderTarget(
                            factory,
                            new D2D.RenderTargetProperties()
                            {
                                MinimumFeatureLevel = D2D.FeatureLevel.Direct3D10,
                                PixelFormat = new D2D.PixelFormat(
                                    DXGI.Format.B8G8R8A8_UNorm,
                                    D2D.AlphaMode.Premultiplied),
                                Type = D2D.RenderTargetType.Software,
                                Usage = D2D.RenderTargetUsage.ForceBitmapRemoting
                            },
                            new D2D.WindowRenderTargetProperties()
                            {
                                Handle = this.Handle,
                                PixelSize = this.Size,
                                PresentOptions = D2D.PresentOptions.Immediately
                            });
                        m_graphics = new Graphics2D(m_renderTarget);
                        m_canvasType = CanvasType.Software;
                    }
                }
            }
            catch (Exception ex)
            {
                //Dispose resources
                m_renderTarget = GraphicsHelper.DisposeGraphicsObject(m_renderTarget);
            }
        }

        /// <summary>
        /// Called when the control's size has changed.
        /// </summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (m_renderTarget != null)
            {
                if ((m_renderTarget.PixelSize.Width != this.Width) ||
                   (m_renderTarget.PixelSize.Height != this.Height))
                {
                    string oldSizeString = "(" + m_renderTarget.PixelSize.Width.ToString() + "," + m_renderTarget.PixelSize.Height + ")";
                    string newSizeString = "(" + this.Width + "," + this.Height + ")";
                    //GraphicsCore.Current.LogDebug("Changing size of Direct2D RenderTarget from " + oldSizeString + " to " + newSizeString + "..", LoggingEntryMarker.Nothing, "Gui", "H: " + this.Handle.ToString());

                    try
                    { 
                        m_renderTarget.Resize(new Size(this.Width, this.Height));
                        //GraphicsCore.Current.LogDebug("Successfully changed size of Direct2D RenderTarget!", LoggingEntryMarker.Success, "Gui", "H: " + this.Handle.ToString());
                    }
                    catch (Exception ex)
                    {
                        //Disable Direct2D rendering
                        m_canvasType = CanvasType.None;

                        //Dispose resources
                        m_renderTarget = GraphicsHelper.DisposeGraphicsObject(m_renderTarget);
                    }
                }
            }
        }

        /// <summary>
        /// Called when the control's handle is destroyed.
        /// </summary>
        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);

            //Dispose resources
            m_renderTarget = GraphicsHelper.DisposeGraphicsObject(m_renderTarget);
            m_graphics = null;
        }

        /// <summary>
        /// Called when background color has changed.
        /// </summary>
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);

            m_backgroundBrush = GraphicsHelper.DisposeGraphicsObject(m_backgroundBrush);
            m_backgroundBrush = new SolidBrush(this.BackColor);
        }

        /// <summary>
        /// Called when control has to paint itself.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (m_renderTarget != null)
            {
                //Direct2D is initialized, so draw using Direct2D
                OnDirect2DPaint(m_renderTarget);
            }
            else
            {
                //Perform fallback rendering using Gdi
                e.Graphics.FillRectangle(m_backgroundBrush, new Rectangle(0, 0, this.Width, this.Height));
            }
        }

        /// <summary>
        /// Called when Direct2D rendering should be done.
        /// </summary>
        /// <param name="renderTarget">The render-target to use.</param>
        protected internal virtual void OnDirect2DPaint(D2D.RenderTarget renderTarget)
        {
            renderTarget.BeginDraw();
            try
            {
                renderTarget.Clear(new SlimDX.Color4(this.BackColor));

                //Call external painting
                RaiseDirect2DPaint(m_graphics);
            }
            finally
            {
                renderTarget.EndDraw();
            }
        }

        /// <summary>
        /// Raises the Paint2D event.
        /// </summary>
        protected internal void RaiseDirect2DPaint()
        {
            if (m_graphics != null)
            {
                if (Paint2D != null) { Paint2D(this, new Direct2DPaintEventArgs(m_graphics)); }
            }
        }

        /// <summary>
        /// Raises the Paint2D event.
        /// </summary>
        /// <param name="graphics2D">The Graphics2D object used for painting.</param>
        protected internal void RaiseDirect2DPaint(Graphics2D graphics2D)
        {
            if (Paint2D != null) { Paint2D(this, new Direct2DPaintEventArgs(graphics2D)); }
        }

        /// <summary>
        /// Gets the type of the canvas object.
        /// </summary>
        [Category("Rendering")]
        public CanvasType CanvasType
        {
            get { return m_canvasType; }
        }

        /// <summary>
        /// Gets current RenderTarget object.
        /// </summary>
        [Browsable(false)]
        internal D2D.RenderTarget RenderTarget
        {
            get { return m_renderTarget; }
        }

        /// <summary>
        /// Gets current graphics object.
        /// </summary>
        [Browsable(false)]
        internal Graphics2D Graphics2D
        {
            get { return m_graphics; }
        }
    }
}
