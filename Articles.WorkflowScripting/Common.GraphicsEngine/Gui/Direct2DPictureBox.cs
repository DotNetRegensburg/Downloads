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
using Common.GraphicsEngine.Core;

//Some namespace mappings
using GDI  = System.Drawing;
using D2D  = SlimDX.Direct2D;
using DXGI = SlimDX.DXGI;

namespace Common.GraphicsEngine.Gui
{
    public partial class Direct2DPictureBox : Direct2DCanvas
    {
        private PictureBoxSizeMode m_sizeMode;
        private bool m_direct2DBitmapInvalid;
        private GDI.Bitmap m_gdiBitmap;
        private D2D.Bitmap m_direct2DBitmap;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public Direct2DPictureBox()
        {
            m_sizeMode = PictureBoxSizeMode.CenterImage;

            //Mark Direct2D bitmap as invalid
            m_direct2DBitmapInvalid = true;
        }

        /// <summary>
        /// Gets an image rectangle based on the given size mode.
        /// </summary>
        private Rectangle GetDrawingRectangle(PictureBoxSizeMode sizeMode, Size imageSize)
        {
            Rectangle result = Rectangle.Empty;

            switch (sizeMode)
            {
                case PictureBoxSizeMode.StretchImage:
                case PictureBoxSizeMode.AutoSize:
                    result = new Rectangle(0, 0, this.Width, this.Height);
                    break;

                case PictureBoxSizeMode.CenterImage:
                    result = new Rectangle(
                        (int)(this.Width / 2f - imageSize.Width / 2f),
                        (int)(this.Height / 2f - imageSize.Height / 2f),
                        imageSize.Width, imageSize.Height);
                    break;

                case PictureBoxSizeMode.Normal:
                    result = new Rectangle(0, 0, imageSize.Width, imageSize.Height);
                    break;

                case PictureBoxSizeMode.Zoom:
                    double scaleFactorX = (double)imageSize.Width / (double)this.Width;
                    double scaleFactorY = (double)imageSize.Height / (double)this.Height;
                    double factorToUse = scaleFactorX > scaleFactorY ? scaleFactorX : scaleFactorY;
                    Size newSize = new Size(
                        (int)(imageSize.Width / factorToUse),
                        (int)(imageSize.Height / factorToUse));
                    result = new Rectangle(
                        (int)(this.Width / 2f - newSize.Width / 2f),
                        (int)(this.Height / 2f - newSize.Height / 2f),
                        newSize.Width, newSize.Height);
                    break;
            }

            return result;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called for each render pass.
        /// </summary>
        protected internal override void OnDirect2DPaint(D2D.RenderTarget renderTarget)
        {
            //Load Direct2D bitmap if it is invalid
            if (m_direct2DBitmapInvalid && (m_gdiBitmap != null))
            {
                if ((m_direct2DBitmap != null) &&
                    (m_direct2DBitmap.PixelSize.Width == m_gdiBitmap.Width) &&
                    (m_direct2DBitmap.PixelSize.Height == m_gdiBitmap.Height))
                {
                    GraphicsHelper.SetBitmapContents(m_direct2DBitmap, m_gdiBitmap);
                }
                else
                {
                    m_direct2DBitmap = GraphicsHelper.DisposeGraphicsObject(m_direct2DBitmap);
                    m_direct2DBitmap = GraphicsHelper.LoadBitmap(renderTarget, m_gdiBitmap);
                }
            }
            else if (m_gdiBitmap == null)
            {
                m_direct2DBitmap = GraphicsHelper.DisposeGraphicsObject(m_direct2DBitmap);
            }
            m_direct2DBitmapInvalid = false;

            //Direct2D is initialized, so draw using Direct2D
            renderTarget.BeginDraw();
            try
            {
                renderTarget.Clear(new SlimDX.Color4(this.BackColor));

                if (m_direct2DBitmap != null)
                {
                    renderTarget.DrawBitmap(
                        m_direct2DBitmap,
                        GetDrawingRectangle(m_sizeMode, m_direct2DBitmap.PixelSize));
                }

                //Raise external drawing
                base.RaiseDirect2DPaint();
            }
            finally
            {
                renderTarget.EndDraw();
            }
        }

        /// <summary>
        /// Gets or sets the currently displayed bitmap.
        /// </summary>
        [Category("Rendering")]
        public GDI.Bitmap Image
        {
            get { return m_gdiBitmap; }
            set
            {
                if (m_gdiBitmap != value)
                {
                    m_gdiBitmap = value;
                    m_direct2DBitmapInvalid = true;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the size mode.
        /// </summary>
        [Category("Rendering")]
        public PictureBoxSizeMode SizeMode
        {
            get { return m_sizeMode; }
            set
            {
                if (m_sizeMode != value)
                {
                    m_sizeMode = value;
                    this.Invalidate();
                }
            }
        }
    }
}
