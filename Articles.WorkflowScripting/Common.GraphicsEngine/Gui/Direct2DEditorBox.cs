using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SlimDX;
using Common.GraphicsEngine.Drawing2D;

//Some namespace mappings
using D2D = SlimDX.Direct2D;

namespace Common.GraphicsEngine.Gui
{
    public class Direct2DEditorBox : Direct2DCanvas
    {
        //Members for the editor's components
        private EditorGrid2D m_editorGrid;
        private EditorViewport2D m_editorViewport;

        //Members for mouse controling
        private bool m_isMouseInside;
        private Point m_currentMousePoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="Direct2DEditorBox"/> class.
        /// </summary>
        public Direct2DEditorBox()
        {
            m_editorViewport = new EditorViewport2D();
            m_editorGrid = new EditorGrid2D();
        }

        /// <summary>
        /// Paints all Direct2D content.
        /// </summary>
        /// <param name="renderTarget"></param>
        protected internal override void OnDirect2DPaint(D2D.RenderTarget renderTarget)
        {
            //Direct2D is initialized, so draw using Direct2D
            renderTarget.BeginDraw();
            try
            {
                renderTarget.Clear(new SlimDX.Color4(this.BackColor));

                Graphics2D graphics = base.Graphics2D;
                if (graphics != null)
                {
                    //Renders current screen
                    m_editorGrid.Render(base.Graphics2D, m_editorViewport);
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
        /// Called when user clicks on the convas element.
        /// </summary>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            this.Focus();
        }

        /// <summary>
        /// Called when the size of this control has changed.
        /// </summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            m_editorViewport.UpdateScreenSize((double)this.Width, (double)this.Height);
        }

        /// <summary>
        /// Called when mouse enters the control's area.
        /// </summary>
        protected override void OnMouseEnter(EventArgs e)
        {
            m_isMouseInside = true;
            m_currentMousePoint = this.PointToClient(Cursor.Position);
            this.Cursor = Cursors.Default;

            base.OnMouseEnter(e);
        }

        /// <summary>
        /// Called when mouse leaves the control's area.
        /// </summary>
        protected override void OnMouseLeave(EventArgs e)
        {
            m_isMouseInside = false;
            this.Cursor = Cursors.Default;

            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Called when mouse moves over the control's area.
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (m_isMouseInside)
            {
                switch (e.Button)
                {
                        //Perform simple movement
                    case MouseButtons.Right:
                        this.Cursor = Cursors.Cross;
                        Point currentWay = new Point(
                            e.Location.X - m_currentMousePoint.X,
                            e.Location.Y - m_currentMousePoint.Y);
                        m_editorViewport.Move(
                            currentWay.X / m_editorViewport.ZoomFactor, 
                            currentWay.Y / m_editorViewport.ZoomFactor);
                        this.Invalidate();
                        break;

                        //Perform default logic
                    default:
                        this.Cursor = Cursors.Default;
                        break;
                }
                m_currentMousePoint = e.Location;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Called when user wants to scroll.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            m_editorViewport.UpdateZoomFactor(
                m_editorViewport.ZoomFactor + ((e.Delta / 1200.0) * m_editorViewport.ZoomFactor));
            this.Invalidate();
        }

        /// <summary>
        /// Called when user presses one of the mouse buttons.
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Called when user does not press one of the mouse buttons any more.
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Gets the viewport to use.
        /// </summary>
        public EditorViewport2D Viewport
        {
            get { return m_editorViewport; }
        }

        /// <summary>
        /// Gets the grid object.
        /// </summary>
        public EditorGrid2D Grid
        {
            get { return m_editorGrid; }
        }
    }
}
