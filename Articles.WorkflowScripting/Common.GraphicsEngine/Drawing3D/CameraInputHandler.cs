using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Common.GraphicsEngine.Drawing3D
{
    public class CameraInputHandler
    {
        private const float MOVEMENT = 0.3f;

        private Control m_targetControl;
        private Camera m_targetCamera;

        private Point m_lastMousePoint;
        private bool m_isMouseInside;
        private bool m_isMovementEnabled;

        /// <summary>
        /// Attaches to the given target control and camera.
        /// </summary>
        /// <param name="targetControl">The target control.</param>
        /// <param name="targetCamera">The target camera.</param>
        public static CameraInputHandler Attach(Control targetControl, Camera targetCamera)
        {
            CameraInputHandler result = new CameraInputHandler();

            result.m_targetCamera = targetCamera;
            result.m_targetControl = targetControl;

            targetControl.MouseLeave += new EventHandler(result.OnTargetControlMouseLeave);
            targetControl.MouseEnter += new EventHandler(result.OnTargetControlMouseEnter);
            targetControl.MouseMove += new MouseEventHandler(result.OnTargetControlMouseMove);
            targetControl.MouseWheel += new MouseEventHandler(result.OnTargetControlMouseWheel);
            targetControl.KeyDown += new KeyEventHandler(result.OnTargetControlKeyDown);

            return result;
        }

        /// <summary>
        /// Called when mouse enters the area of the target control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnTargetControlMouseEnter(object sender, EventArgs e)
        {
            m_lastMousePoint = m_targetControl.PointToClient(Cursor.Position);
            m_isMouseInside = true;
        }

        /// <summary>
        /// Called when mouse leaves the area of the target control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnTargetControlMouseLeave(object sender, EventArgs e)
        {
            m_lastMousePoint = Point.Empty;
            m_isMouseInside = false;
        }

        /// <summary>
        /// Called when user hits any key.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        private void OnTargetControlKeyDown(object sender, KeyEventArgs e)
        {
            //Handle key input
            if (m_isMovementEnabled)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                    case Keys.W:
                        m_targetCamera.Zoom(MOVEMENT);
                        break;

                    case Keys.Down:
                    case Keys.S:
                        m_targetCamera.Zoom(-MOVEMENT);
                        break;

                    case Keys.Left:
                    case Keys.A:
                        m_targetCamera.Strave(-MOVEMENT);
                        break;

                    case Keys.Right:
                    case Keys.D:
                        m_targetCamera.Strave(MOVEMENT);
                        break;

                    case Keys.Q:
                        m_targetCamera.UpDown(MOVEMENT);
                        break;

                    case Keys.E:
                        m_targetCamera.UpDown(-MOVEMENT);
                        break;
                }
            }

            m_targetControl.Invalidate();
        }

        /// <summary>
        /// Called when user zoom's using the mouse.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void OnTargetControlMouseWheel(object sender, MouseEventArgs e)
        {
            if (m_isMouseInside)
            {
                if (m_isMovementEnabled)
                {
                    m_targetCamera.Zoom(e.Delta / 100f);
                }

                m_targetControl.Invalidate();
            }
        }

        /// <summary>
        /// Called when location of the mouse cursor changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void OnTargetControlMouseMove(object sender, MouseEventArgs e)
        {
            if (m_isMouseInside)
            {
                Point moving = new Point(e.X - m_lastMousePoint.X, e.Y - m_lastMousePoint.Y);
                m_lastMousePoint = e.Location;

                if (m_isMovementEnabled)
                {
                    switch (e.Button)
                    {
                        case MouseButtons.Right:
                            m_targetCamera.Rotate(-moving.X / 200f, -moving.Y / 200f);
                            break;

                        case MouseButtons.Left:
                            m_targetCamera.Strave(moving.X / 50f);
                            m_targetCamera.UpDown(-moving.Y / 50f);
                            break;
                    }
                }

                m_targetControl.Invalidate();
            }
        }
    }
}
