using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Common.GraphicsEngine.Gui
{
    public partial class Direct3D11SceneView
	{
        private const float MOVEMENT = 0.3f;

        private Point m_lastMousePoint;
        private bool m_isMouseInside;
        private bool m_isMovementEnabled;

        /// <summary>
        /// Called when the mouse enters the screen.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            m_lastMousePoint = base.PointToClient(Cursor.Position);
            m_isMouseInside = true;
        }

        /// <summary>
        /// Called when user clicks on this panel.
        /// </summary>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            this.Focus();
        }

        /// <summary>
        /// Called when the mouse leaves the screen.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            m_lastMousePoint = Point.Empty;
            m_isMouseInside = false;
        }

        /// <summary>
        /// Called when the mouse moves on the screen.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (m_isMouseInside)
            {
                Point moving = new Point(e.X - m_lastMousePoint.X, e.Y - m_lastMousePoint.Y);
                m_lastMousePoint = e.Location;

                if (m_isMovementEnabled)
                {
                    switch (e.Button)
                    {
                        case MouseButtons.Right:
                            m_camera.Rotate(-moving.X / 200f, -moving.Y / 200f);
                            break;

                        case MouseButtons.Left:
                            m_camera.Strave(moving.X / 50f);
                            m_camera.UpDown(-moving.Y / 50f);
                            break;
                    }
                }

                this.Invalidate();
            }
        }

        /// <summary>
        /// Called when mouse wheel is used.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (m_isMouseInside)
            {
                if (m_isMovementEnabled)
                {
                    m_camera.Zoom(e.Delta / 100f);
                }

                this.Invalidate();
            }
        }

        /// <summary>
        /// Called when a key is down.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            //Handle key input
            if (m_isMovementEnabled)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                    case Keys.W:
                        m_camera.Zoom(MOVEMENT);
                        break;

                    case Keys.Down:
                    case Keys.S:
                        m_camera.Zoom(-MOVEMENT);
                        break;

                    case Keys.Left:
                    case Keys.A:
                        m_camera.Strave(-MOVEMENT);
                        break;

                    case Keys.Right:
                    case Keys.D:
                        m_camera.Strave(MOVEMENT);
                        break;

                    case Keys.Q:
                        m_camera.UpDown(MOVEMENT);
                        break;

                    case Keys.E:
                        m_camera.UpDown(-MOVEMENT);
                        break;
                }
            }

            this.Invalidate();
        }

        /// <summary>
        /// Is movement enabled?
        /// </summary>
        [Category("Rendering")]
        public bool IsMovementEnabled
        {
            get { return m_isMovementEnabled; }
            set { m_isMovementEnabled = value; }
        }
	}
}
